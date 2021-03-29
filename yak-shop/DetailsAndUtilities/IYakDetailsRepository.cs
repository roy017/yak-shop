using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yak_shop.DetailsAndUtilities
{
    public interface IYakDetailsRepository
    {
        public YakDetails GetYak(int yakId);

        
        //List<YakDetails> GetAll();

        //void AddYak(YakDetails yak);

        //YakDetails FindYakData(int id);
        //void UpdateYakData(YakDetails yak);

        //void RemoveYakData(int id);
    }
}
