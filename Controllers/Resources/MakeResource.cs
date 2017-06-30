using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Vega.Controllers.Resources
{
    public class MakeResource : KeyValuePairResource
    {
        //public int Id { get; set; }

        //public string Name { get; set; }
        //public ICollection<ModelResource> Models { get; set; }
        //public MakeResource()
        //{
        //    Models = new Collection<ModelResource>();
        //}

        public ICollection<KeyValuePairResource> Models { get; set; }

        public MakeResource()
        {
            Models = new Collection<KeyValuePairResource>();
        }
    }
}
