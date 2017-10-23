using System;
using System.Collections.Generic;
using dawn_of_worlds.Main;
using Nest;
using dawn_of_worlds.WorldClasses;

namespace dawn_of_worlds.Log
{
    class ElasticSearch
    {
        public ElasticClient Client { get; set; }

        public ElasticSearch()
        {
            Client = new ElasticClient();

        }

        public void IndexProvinces()
        {
            var descriptor = new CreateIndexDescriptor("province")
                    .Mappings(ms => ms
                        .Map<Province>(m => m.AutoMap())
            );
            var index_response = Client.Index(descriptor);
        }

    }
}
