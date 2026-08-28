using System;
using System.Collections.Generic;
using System.Text;

namespace EShopData.Common
{
    public class ConvertingHelper
    {
        public List<int> GetIdsOfChosenOptions(bool[] chosenOptions, int[] allIds)
        {
            var ids = new List<int>();

            for (int i = 0; i < chosenOptions.Length; i++)
            {
                if (chosenOptions[i])
                {
                    ids.Add(allIds[i]);
                }
            }

            return ids;
        }
    }
}
