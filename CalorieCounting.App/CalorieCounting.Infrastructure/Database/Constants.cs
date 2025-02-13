using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalorieCounting.Infrastructure.Database
{
    public static class Constants
    {
        public static class ShadowPropertyNames {
            public const string IsDeleted = "is_deleted";
            public const string CreatedAt = "created_at";
            public const string ModifiedAt = "modified_at";
        }
    }
}
