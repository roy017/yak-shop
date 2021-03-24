using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yak_shop.DetailsAndUtilities
{
    public class IYakDetailsRepository
    {
        public virtual List<YakDetails> GetAll()
        {
            throw new System.NotImplementedException();
        }
        public virtual void AddYak(YakDetails yak)
        {
            throw new System.NotImplementedException();
        }
        public virtual YakDetails FindYakData(int id)
        {
            throw new System.NotImplementedException();
        }
        public virtual void UpdateYakData(YakDetails yak)
        {
            throw new System.NotImplementedException();
        }

        public virtual void RemoveYakData(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
