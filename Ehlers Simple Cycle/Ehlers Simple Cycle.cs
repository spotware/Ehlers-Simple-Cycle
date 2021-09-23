using cAlgo.API;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None), Cloud("Cycle", "Trigger"), Levels(0)]
    public class EhlersSimpleCycle : Indicator
    {
        private IndicatorDataSeries _smooth;

        [Parameter("Source")]
        public DataSeries Source { get; set; }

        [Parameter("Alpha", DefaultValue = 0.07, Step = 0.01)]
        public double Alpha { get; set; }

        [Output("Cycle", LineColor = "Green", PlotType = PlotType.Line)]
        public IndicatorDataSeries Cycle { get; set; }

        [Output("Trigger", LineColor = "Red", PlotType = PlotType.Line)]
        public IndicatorDataSeries Trigger { get; set; }

        protected override void Initialize()
        {
            _smooth = CreateDataSeries();
        }

        public override void Calculate(int index)
        {
            _smooth[index] = (Source[index] + 2 * Source[index - 1] + 2 * Source[index - 2] + Source[index - 3]) / 6.0;

            if (index < 7)
            {
                Cycle[index] = (Source[index] - 2 * Source[index - 1] + Source[index - 2]) / 4.0;
            }
            else
            {
                Cycle[index] = (1 - .5 * Alpha) * (1 - .5 * Alpha) * (_smooth[index] - 2 * _smooth[index - 1] + _smooth[index - 2]) + 2 * (1 - Alpha) * Cycle[index - 1] - (1 - Alpha) * (1 - Alpha) * Cycle[index - 2];
            }

            Trigger[index] = Cycle[index - 1];
        }
    }
}