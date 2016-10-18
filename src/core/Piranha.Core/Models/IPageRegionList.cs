﻿/*
 * Copyright (c) 2016 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

namespace Piranha.Models
{
    /// <summary>
    /// Interface for accessing the meta data of a region list.
    /// </summary>
    public interface IPageRegionList
    {
        /// <summary>
        /// Gets/sets the page type id.
        /// </summary>
        string TypeId { get; set; }

        /// <summary>
        /// Gets/sets the region id.
        /// </summary>
        string RegionId { get; set; }
    }
}
