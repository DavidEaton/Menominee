using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MenomineePlayWASM.Server.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            //CreateMap<SomeClass, SomeInstance>()
            //    .ForMember(x => x.SomeField, option => option.Ignore());
        }
    }
}
