using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using System.Diagnostics;
using System.Data.SqlClient;
using Dapper;

namespace yak_shop.DetailsAndUtilities
{
    public class YakUtilities
    {
        const int MAX_YAK_AGE_DAYS = 1000;
        
        public StockDetails GetHerdStatistics( ref List<YakDetails> herdInfo, int days)
        {
            StockDetails stockInfo = new StockDetails();

            int totalSkins = 0;
            double totalMilk = 0;
            foreach (var yak in herdInfo)
            {
                yak.ageLastShaved = yak.Age;
                float ageInDaysNow = yak.Age * 100;
                totalMilk += GetMilk(days, yak, ageInDaysNow);
                totalSkins += GetSkins(days, yak, ref ageInDaysNow);

                yak.Age +=  ((float)days / 100);
                yak.ageLastShaved = ageInDaysNow / 100;
                    
            }
            
            stockInfo.Skins = totalSkins;
            stockInfo.Milk = totalMilk;
            return stockInfo;
        }

        private static double GetMilk(int days, YakDetails yak, float ageInDaysNow)
        {
            float yakAgePlusDays = ageInDaysNow + days;
            int elapsedDays;
            double totalMilk = 0f;
            if (yakAgePlusDays >= MAX_YAK_AGE_DAYS)
            {
                elapsedDays = ((MAX_YAK_AGE_DAYS - 1) - (int)(yak.Age * 100));
            }
            else
                elapsedDays = days;

            for (int i = 0; i < elapsedDays; i++)
            {
                totalMilk += (50 - ((ageInDaysNow + i) * 0.03));
            }
            return totalMilk;
        }

        private static int GetSkins(int days, YakDetails yak, ref float ageInDaysNow)
        {
            int countSkins = 1;
            int lastShaved = (int)(yak.ageLastShaved * 100);

            for (int i = 1; i <= days; i++)
            {
                int ageNow = (int)(yak.Age * 100 + i);
                if (ageNow >= MAX_YAK_AGE_DAYS)
                    break;
                if (canBeShaved(ageNow, lastShaved))
                {
                    countSkins++;
                    lastShaved = ageNow;
                }
            }
            ageInDaysNow = (float)lastShaved;
            
            return countSkins;

        }

        private static bool canBeShaved(int ageNow, int lastShaved)
        {
            bool flag = false;
            double daysNeededBeforeShave = (8 + (ageNow) * 0.01);

            if ((double)ageNow - (double)lastShaved > daysNeededBeforeShave)
                flag = true;
            return flag;
        }
    }
}
