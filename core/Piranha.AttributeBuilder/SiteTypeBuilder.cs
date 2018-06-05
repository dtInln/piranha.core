﻿/*
 * Copyright (c) 2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Piranha.Models;

namespace Piranha.AttributeBuilder
{
    public class SiteTypeBuilder : ContentTypeBuilder<SiteTypeBuilder, SiteType>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="api">The current api</param>        
        public SiteTypeBuilder(IApi api) : base(api) { }

        /// <summary>
        /// Builds the site types.
        /// </summary>
        public override SiteTypeBuilder Build()
        {
            foreach (var type in _types)
            {
                var siteType = GetContentType(type);

                if (siteType != null)
                {
                    siteType.Ensure();
                    _api.SiteTypes.Save(siteType);
                }
            }
            return this;
        }

        /// <summary>
        /// Deletes all site types in the database that doesn't
        ///  exist in the import,
        /// </summary>
        /// <returns>The builder</returns>
        public SiteTypeBuilder DeleteOrphans()
        {
            var orphans = new List<SiteType>();
            var importTypes = new List<SiteType>();

            // Get all site types added for import.
            foreach (var type in _types)
            {
                var importType = GetContentType(type);

                if (importType != null)
                    importTypes.Add(importType);
            }

            // Get all previously imported page types.
            foreach (var siteType in _api.SiteTypes.GetAll())
            {
                if (!importTypes.Any(t => t.Id == siteType.Id))
                    orphans.Add(siteType);
            }

            // Delete all orphans.
            foreach (var siteType in orphans)
            {
                _api.SiteTypes.Delete(siteType);
            }
            return this;
        }

        #region Private methods
        /// <summary>
        /// Gets the possible page type for the given type.
        /// </summary>
        /// <param name="type">The type</param>
        /// <returns>The page type</returns>
        protected override SiteType GetContentType(Type type)
        {
            var attr = type.GetTypeInfo().GetCustomAttribute<SiteTypeAttribute>();

            if (attr != null)
            {
                if (string.IsNullOrWhiteSpace(attr.Id))
                    attr.Id = type.Name;

                if (!string.IsNullOrEmpty(attr.Id) && !string.IsNullOrEmpty(attr.Title))
                {
                    var siteType = new SiteType
                    {
                        Id = attr.Id,
                        CLRType = type.GetTypeInfo().AssemblyQualifiedName,
                        Title = attr.Title,
                        ContentTypeId = App.ContentTypes.GetId(type)
                    };

                    var regionTypes = new List<Tuple<int?, RegionType>>();

                    foreach (var prop in type.GetProperties(App.PropertyBindings))
                    {
                        var regionType = GetRegionType(prop);

                        if (regionType != null)
                        {
                            regionTypes.Add(regionType);
                        }
                    }
                    regionTypes = regionTypes.OrderBy(t => t.Item1).ToList();

                    // First add sorted regions
                    foreach (var regionType in regionTypes.Where(t => t.Item1.HasValue))
                        siteType.Regions.Add(regionType.Item2);
                    // Then add the unsorted regions
                    foreach (var regionType in regionTypes.Where(t => !t.Item1.HasValue))
                        siteType.Regions.Add(regionType.Item2);

                    return siteType;
                }
            }
            else
            {
                throw new ArgumentException($"Title is mandatory in SiteTypeAttribute. No title provided for {type.Name}");
            }
            return null;
        }
        #endregion
    }
}
