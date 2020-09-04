﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.AppMagic.Authoring.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Linq;

namespace PAModel
{
    // Various data that we can save for round-tripping.
    // Everything here is optional!!
    // Only be written during MsApp. Opaque for source file. 
    class Entropy
    {
        // Json serialize these. 
        public Dictionary<string, string> TemplateVersions { get; set; }  = new Dictionary<string, string>();
        public DateTime? HeaderLastSavedDateTimeUTC { get; set; }
        public string OldLogoFileName { get; set; }

        // To fully round-trip, we need to preserve array order for the various un-ordered arrays that we may split apart.         
        public Dictionary<string, int> OrderDataSource { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> OrderComponentMetadata { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> OrderTemplate { get; set; } = new Dictionary<string, int>();


        public int GetOrder(DataSourceEntry dataSource)
        {
            return this.OrderDataSource.GetOrDefault<string,int>(dataSource.GetUniqueName(), -1);
        }
        public void Add(DataSourceEntry entry, int? order)
        {
            if (order.HasValue)
            {
                this.OrderDataSource[entry.GetUniqueName()] = order.Value;
            }
        }

        public int GetOrder(ComponentsMetadataJson.Entry entry)
        {
            return this.OrderComponentMetadata.GetOrDefault<string, int>(entry.TemplateName, -1);
        }
        public void Add(ComponentsMetadataJson.Entry entry, int order)
        {
            this.OrderComponentMetadata[entry.TemplateName] = order;
        }

        public int GetOrder(TemplateMetadataJson entry)
        {
            return this.OrderTemplate.GetOrDefault<string, int>(entry.Name, -1);
        }
        public void Add(TemplateMetadataJson entry, int order)
        {
            this.OrderTemplate[entry.Name] = order;
        }


        public void SetHeaderLastSaved(DateTime? x)
        {
            this.HeaderLastSavedDateTimeUTC = x;            
        }
        public DateTime? GetHeaderLastSaved()
        {
            return this.HeaderLastSavedDateTimeUTC;
        }

        public void SetTemplateVersion(string dataComponentGuid, string version)
        {
            TemplateVersions[dataComponentGuid] = version;
        }

        public string GetTemplateVersion(string dataComponentGuid)
        {
            string version;
            TemplateVersions.TryGetValue(dataComponentGuid, out version);

            // Version string is ok to be null. 
            // DateTime.Now.ToUniversalTime().Ticks.ToString();
            return version;
        }

        public void SetLogoFileName(string oldLogoName)
        {
            this.OldLogoFileName = oldLogoName;
        }        
    }
}
