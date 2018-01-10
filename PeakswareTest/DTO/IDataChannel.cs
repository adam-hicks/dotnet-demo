using System.Collections.Generic;

namespace PeakswareTest.DTO
{
    public interface IDataChannel
    {
        Dictionary<double, ushort> getData();
        void setData(Dictionary<double, ushort> rawData);
        void calculateMaxEffort(int effortTimeMinutes);

        void calculateAllEfforts();
    }
}