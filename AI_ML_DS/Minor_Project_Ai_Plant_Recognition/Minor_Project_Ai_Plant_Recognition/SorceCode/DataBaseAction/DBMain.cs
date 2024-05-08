using Minor_Project_Ai_Plant_Recognition.SorceCode.DataStructure;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataBaseAction
{
    internal class DBMain
    {
        private Dictionary<int, string> speciesDictIdx = new Dictionary<int, string>();

        public void SpeciesDictInit(Dictionary<int, string> speciesDict)
        {
            DBReader dbReader = new DBReader();

            string speceisIDXDuery = "SELECT idx, sname FROM species_idx;";

            dbReader.IndexReader(speceisIDXDuery, speciesDict);

            WriteLine("SpeciesIdx to Dict: done");
        }

        public void DataPaserFromOrignalDirAndFeeder()
        {
            SpeciesDictInit(speciesDictIdx);
            DBFeeder dbFeeder = new DBFeeder();
            string path = "D:\\Dataset\\medai\\DataOrignal";
            string[] files = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

            List<OrignalImgPath.ImgPathOrignal> imgData = new List<OrignalImgPath.ImgPathOrignal>();

            foreach (string file in files)
            {
                string[] parts = file.Split('\\');

                string species = parts[parts.Length - 2];
                string catagory = parts[parts.Length - 3];

                int speciesID = 0;
                int catagoryID = 0;
                foreach (KeyValuePair<int, string> spe in speciesDictIdx)
                {
                    if (spe.Value == species)
                    {
                        speciesID = spe.Key;
                        break;
                    }
                }

                if (catagory == "Medicinal_leaf_dataset")
                {
                    catagoryID = 1;
                }
                else if (catagory == "Medicinal_plant_dataset")
                {
                    catagoryID = 2;
                }
                else
                {
                    WriteLine("Invalid catagory");
                }

                OrignalImgPath.ImgPathOrignal imgPathOrignal = new OrignalImgPath.ImgPathOrignal
                {
                    catagoryId = catagoryID,
                    speciesId = speciesID,
                    imgPath = file
                };

                imgData.Add(imgPathOrignal);
            }

            dbFeeder.DataFeederToOrignalPathtable(imgData);

            WriteLine("Data Feeding to Orignal Path Table: done");
        }

        public void DataParseFromOrignalPathTable(List<OrignalImgPath.ImgPathOrignal> imgPath)
        {
            DBReader dbReader = new DBReader();
            string SQLQuery = "SELECT catagory, species, imgpath FROM pathtableorignal;";
            dbReader.orignalImgPathReader(SQLQuery, imgPath);
            WriteLine("Data Parsing from Orignal Path Table: done");
        }
    }
}