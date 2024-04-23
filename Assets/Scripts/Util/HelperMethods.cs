using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class HelperMethods
    {
        public static string GenerateUniqueId()
        {
            DateTime now = DateTime.Now;
            Guid guid = Guid.NewGuid();
            string uniqueId = $"{now:MMddHHmm}{guid:N}"[..8];

            return uniqueId;
        }
    }
}