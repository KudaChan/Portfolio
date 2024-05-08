using Minor_Project_Ai_Plant_Recognition.SorceCode.DataBaseAction;
using Minor_Project_Ai_Plant_Recognition.SorceCode.DataStructure;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.PreProcessing
{
    internal class Preprocessing
    {
        private Dictionary<int, string> speciesDict = new Dictionary<int, string>();
        private List<OrignalImgPath.ImgPathOrignal> imgData = new List<OrignalImgPath.ImgPathOrignal>();

        public void PreProcessing()
        {
            DBMain dbMain = new DBMain();
            dbMain.SpeciesDictInit(speciesDict);

            dbMain.DataParseFromOrignalPathTable(imgData);

            foreach (OrignalImgPath.ImgPathOrignal img in imgData)
            {
                WriteLine(img.catagoryId + "-" + img.speciesId + "-" + img.imgPath);
            }
        }
    }
}