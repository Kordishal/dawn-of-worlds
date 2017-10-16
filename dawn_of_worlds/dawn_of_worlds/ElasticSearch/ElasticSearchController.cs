using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using Nest;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.ElasticSearch
{
    class ElasticSearchController
    {

        ConnectionSettings connectionSettings { get; set; }
        ElasticClient elasticClient { get; set; }


        public ElasticSearchController()
        {
            connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200"));
            elasticClient = new ElasticClient(connectionSettings);

            elasticClient.CreateIndex("map_record", ms => ms.Mappings(x => x.Map<Province>(m => m.AutoMap())));
           
        }



    }
}
