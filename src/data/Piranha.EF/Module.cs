﻿/*
 * Copyright (c) 2016 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using AutoMapper;

namespace Piranha.EF
{
    /// <summary>
    /// The Entity Framework module.
    /// </summary>
    public class Module : Extend.IModule
    {
        /// <summary>
        /// Gets the mapper.
        /// </summary>
        public static IMapper Mapper { get; private set; }

        /// <summary>
        /// Initializes the module.
        /// </summary>
        public void Init() {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Data.Category, Models.Category>();
                cfg.CreateMap<Data.Category, Models.CategoryModel>();

                cfg.CreateMap<Models.Category, Data.Category>()
                    .ForMember(m => m.Id, o => o.Ignore())
                    .ForMember(m => m.ArchiveTitle, o => o.Ignore())
                    .ForMember(m => m.ArchiveKeywords, o => o.Ignore())
                    .ForMember(m => m.ArchiveDescription, o => o.Ignore())
                    .ForMember(m => m.ArchiveRoute, o => o.Ignore())
                    .ForMember(m => m.Created, o => o.Ignore())
                    .ForMember(m => m.LastModified, o => o.Ignore());
                cfg.CreateMap<Models.CategoryModel, Data.Category>()
                    .ForMember(m => m.Id, o => o.Ignore())
                    .ForMember(m => m.Created, o => o.Ignore())
                    .ForMember(m => m.LastModified, o => o.Ignore());

                cfg.CreateMap<Data.Page, Models.PageBase>();
                cfg.CreateMap<Models.PageBase, Data.Page>()
                    .ForMember(m => m.Id, o => o.Ignore())
                    .ForMember(m => m.Created, o => o.Ignore())
                    .ForMember(m => m.LastModified, o => o.Ignore())
                    .ForMember(m => m.PageType, o => o.Ignore())
                    .ForMember(m => m.Fields, o => o.Ignore());

                cfg.CreateMap<Data.Post, Models.PostModel>()
                    .ForMember(m => m.Permalink, o => o.MapFrom(p => p.Category.Slug + "/" + p.Slug))
                    .ForMember(m => m.Category, o => o.Ignore())
                    .ForMember(m => m.Tags, o => o.Ignore());
            });

            config.AssertConfigurationIsValid();
            Mapper = config.CreateMapper();
        }
    }
}
