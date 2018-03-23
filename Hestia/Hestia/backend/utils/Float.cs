using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace Hestia.backend.utils
{
    class Float : Object
    {
        private float value;
        public float Value { get; set; }

        public Float (String value)
        {
            this.value = float.Parse(value);
        }
    }
}