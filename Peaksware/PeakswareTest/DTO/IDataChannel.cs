using System.Collections.Generic;

namespace PeakswareTest.DTO
{
    public interface IDataChannel
    {
        Dictionary<int, double> getMaxEfforts();
        Dictionary<double, ushort> getData();
        void setData(Dictionary<double, ushort> rawData);
        void calculateMaxEffort(int effortTimeMinutes);

        void calculateAllEfforts();
    }
}