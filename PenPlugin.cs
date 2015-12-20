﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplifiedPaint
{
    class PenPlugin : IToolPlugin
    {
        public string Author
        {
            get
            {
                return "krystian@swidurski.pl";
            }
        }

        public string Icon
        {
            get
            {
                return "F1 M 21.5367,46.0076L 19,57L 29.3932,54.6016C 28.0768,50.6411 25.8696,47.0904 21.5367,46.0076 Z M 39,53L 69.4259,22.5741C 67.0871,17.8183 63.7005,13.6708 59.5673,10.4327L 31,39L 31,45L 39,45L 39,53 Z M 29,38L 57.8385,9.1615C 56.438,8.19625 54.9638,7.33038 53.4259,6.57407L 24,36L 24,38L 29,38 Z ";
            }
        }

        public string Name
        {
            get
            {
                return "Pen";
            }
        }

        public IAbstractTool Tool
        {
            get
            {
                return new PenTool();
            }
        }

        public string Version
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string GetDescription(string langCode)
        {
            if (langCode.Contains("pl"))
                return "Rysuje dowolne kształty";
            return "Draws anything you wish";
        }
    }
}
