namespace Crop_Recommendation_Project.src.DataStructure
{
    internal class CropDataStructure
    {
        public float N { get; set; }
        public float P { get; set; }
        public float K { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Ph { get; set; }
        public float Rainfall { get; set; }
        public string Label { get; set; }
    }

    internal class CropIdxDataStructure
    {
        public float N { get; set; }
        public float P { get; set; }
        public float K { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Ph { get; set; }
        public float Rainfall { get; set; }
        public int Label_Idx { get; set; }
    }
}