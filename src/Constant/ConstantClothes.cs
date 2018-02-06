/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;

namespace Serverside.Constant
{
    public class ConstantClothes
    {
        public static readonly List<string> Mens = new List<string>
        {
            "0","1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17","18","19","20","42","43","44"
        };

        public static readonly List<string> Womans = new List<string>
        {
            "21","22","23","24","25","26","27","28","29","30","31","32","33","34","35","36","37","38","39","40","41"
        };

        public static List<string> MenLegs
        {
            get
            {
                List<string> legs = new List<string>();

                for (int i = 0; i < 85; i++)
                {
                    legs.Add(i.ToString());
                }

                //Wyrzucam wszystkie kurwo-stroje z kreatora
                legs.RemoveAt(11);
                legs.RemoveAt(14);
                legs.RemoveAt(21);
                legs.RemoveAt(40);
                legs.RemoveAt(61);
                legs.Sort();

                return legs;
            }
        }

        public static List<string> WomanLegs
        {
            get
            {
                List<string> legs = new List<string>();

                for (int i = 0; i < 88; i++)
                {
                    legs.Add(i.ToString());
                }

                //Wyrzucam wszystkie kurwo-stroje z kreatora
                legs.RemoveAt(13);
                legs.RemoveAt(15);
                legs.RemoveAt(17);
                legs.RemoveAt(19);
                legs.RemoveAt(20);
                legs.RemoveAt(21);
                legs.RemoveAt(22);
                legs.RemoveAt(46);
                legs.RemoveAt(56);
                legs.RemoveAt(59);
                legs.RemoveAt(62);
                legs.RemoveAt(63);
                legs.Sort();

                return legs;
            }
        }

        public static List<string> MenFeet
        {
            get
            {
                List<string> feets = new List<string>();

                for (int i = 0; i < 58; i++)
                {
                    feets.Add(i.ToString());
                }

                return feets;
            }
        }

        public static List<string> WomanFeets
        {
            get
            {
                List<string> feets = new List<string>();

                for (int i = 0; i < 61; i++)
                {
                    feets.Add(i.ToString());
                }

                return feets;
            }
        }

        public static List<string> MenAccesories
        {
            get
            {
                List<string> accesories = new List<string>();

                for (int i = 0; i < 124; i++)
                {
                    accesories.Add(i.ToString());
                }

                //Nic tu nie ma
                accesories.RemoveRange(1, 8);
                accesories.RemoveRange(56, 17);
                //accesories.RemoveRange(95, 13);

                accesories.RemoveAt(13);
                accesories.RemoveAt(14);
                accesories.RemoveAt(15);
                accesories.RemoveAt(84);


                return accesories;
            }
        }

        public static List<string> WomanAccesories
        {
            get
            {
                List<string> accesories = new List<string>();

                for (int i = 0; i < 94; i++)
                {
                    accesories.Add(i.ToString());
                }

                //Nic tu nie ma
                accesories.Remove("43");
                accesories.Remove("44");
                accesories.Remove("45");
                accesories.Remove("46");
                accesories.Remove("47");
                accesories.Remove("48");
                accesories.Remove("49");
                accesories.Remove("50");
                accesories.Remove("51");
                accesories.Remove("52");
                accesories.Remove("63");
                accesories.Remove("74");
                accesories.Remove("75");
                accesories.Remove("76");
                accesories.Remove("77");
                accesories.Remove("78");
                accesories.Remove("79");
                accesories.Remove("80");

                return accesories;
            }
        }


        public static List<string> MenHats
        {
            get
            {
                List<string> hats = new List<string>();

                for (int i = 0; i < 102; i++)
                {
                    hats.Add(i.ToString());
                }

                return hats;
            }
        }

        public static List<string> WomanHats
        {
            get
            {
                List<string> hats = new List<string>();

                for (int i = 0; i < 101; i++)
                {
                    hats.Add(i.ToString());
                }

                return hats;
            }
        }

        public static List<string> MenGlasses
        {
            get
            {
                List<string> glasses = new List<string>();

                for (int i = 0; i < 25; i++)
                {
                    glasses.Add(i.ToString());
                }

                return glasses;
            }
        }

        public static List<string> WomanGlasses
        {
            get
            {
                List<string> glasses = new List<string>();

                for (int i = 0; i < 27; i++)
                {
                    glasses.Add(i.ToString());
                }

                return glasses;
            }
        }

        public static List<string> MenEars
        {
            get
            {
                List<string> ears = new List<string>();

                for (int i = 0; i < 36; i++)
                {
                    ears.Add(i.ToString());
                }

                return ears;
            }
        }

        public static List<string> WomanEars
        {
            get
            {
                List<string> ears = new List<string>();

                for (int i = 0; i < 17; i++)
                {
                    ears.Add(i.ToString());
                }

                return ears;
            }
        }

        public static List<string> MensHairId
        {
            get
            {
                List<string> hairs = new List<string>();

                for (int i = 0; i < 36; i++)
                {
                    hairs.Add(i.ToString());
                }

                //Noktowizja
                hairs.Remove("23");

                return hairs;
            }
        }

        public static List<string> WomansHairId
        {
            get
            {
                List<string> hairs = new List<string>();

                for (int i = 0; i < 38; i++)
                {
                    hairs.Add(i.ToString());
                }

                //Noktowizja
                hairs.Remove("24");

                return hairs;
            }
        }
    }
}