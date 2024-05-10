using Minor_Project_Ai_Plant_Recognition.SorceCode.DataStructure;

namespace Minor_Project_Ai_Plant_Recognition.SorceCode.DataBaseAction
{
    internal class DBMain
    {
        private Dictionary<int, string> speciesDictIdx = new Dictionary<int, string>();
        private Dictionary<int, string> processDictIdx = new Dictionary<int, string>();
        private Dictionary<int, string> modifierDictIdx = new Dictionary<int, string>();

        public void SpeciesDictInit(Dictionary<int, string> speciesDict)
        {
            DBReader dbReader = new DBReader();

            string speceisIDXDuery = "SELECT idx, sname FROM species_idx;";

            dbReader.IndexReader(speceisIDXDuery, speciesDict);

            WriteLine("SpeciesIdx to Dict: done");
        }

        public void ProcessDictInit(Dictionary<int, string> processDict)
        {
            DBReader dbReader = new DBReader();

            string processIDXDuery = "SELECT idx, pname FROM process_idx;";

            dbReader.IndexReader(processIDXDuery, processDict);

            WriteLine("ProcessIdx to Dict: done");
        }

        public void ModifierDictInit(Dictionary<int, string> modifierDict)
        {
            DBReader dbReader = new DBReader();

            string modifierIDXDuery = "SELECT idx, mname FROM modifier_idx;";

            dbReader.IndexReader(modifierIDXDuery, modifierDict);

            WriteLine("ModifierIdx to Dict: done");
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

            WriteLine("Total img count" + imgData.Count);

            WriteLine("Data Feeding to Orignal Path Table: done");
        }

        public void DataParserFromPreprocessedDirAndFeeder()
        {
            if (speciesDictIdx.Count == 0)
            {
                SpeciesDictInit(speciesDictIdx);
            }
            if (processDictIdx.Count == 0)
            {
                ProcessDictInit(processDictIdx);
            }
            if (modifierDictIdx.Count == 0)
            {
                ModifierDictInit(modifierDictIdx);
            }

            DBFeeder dbFeeder = new DBFeeder();
            string path = "D:\\Dataset\\medai\\PreProcessed";
            string[] files = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

            List<PreprocessedPath.ImgPathPreprocessed> imgData = new List<PreprocessedPath.ImgPathPreprocessed>();

            foreach (string file in files)
            {
                string[] parts = file.Split('\\');

                string species = parts[parts.Length - 4];
                string catagory = parts[parts.Length - 5];
                string process = parts[parts.Length - 3];
                string modifier = parts[parts.Length - 2];

                int speciesID = 0;
                int catagoryID = 0;
                int processID = 0;
                int modifierID = 0;

                foreach (KeyValuePair<int, string> spe in speciesDictIdx)
                {
                    if (spe.Value == species)
                    {
                        speciesID = spe.Key;
                        break;
                    }
                }

                foreach (KeyValuePair<int, string> pro in processDictIdx)
                {
                    if (pro.Value == process)
                    {
                        processID = pro.Key;
                        break;
                    }
                }

                foreach (KeyValuePair<int, string> mod in modifierDictIdx)
                {
                    if (mod.Value == modifier)
                    {
                        modifierID = mod.Key;
                        break;
                    }
                }

                if (catagory == "leaf")
                {
                    catagoryID = 1;
                }
                else if (catagory == "plant")
                {
                    catagoryID = 2;
                }
                else
                {
                    WriteLine("Invalid catagory");
                }

                PreprocessedPath.ImgPathPreprocessed imgPathPreprocessed = new PreprocessedPath.ImgPathPreprocessed
                {
                    catagoryId = catagoryID,
                    speciesId = speciesID,
                    preprocessedId = processID,
                    modifierId = modifierID,
                    imgPath = file
                };

                imgData.Add(imgPathPreprocessed);
            }

            dbFeeder.DataFeederToPathtable(imgData);

            WriteLine("Total img count" + imgData.Count);

            WriteLine("Data Feeding to Preprocessed Path Table: done");
        }

        public void DataParseFromOrignalPathTable(List<OrignalImgPath.ImgPathOrignal> imgPath)
        {
            DBReader dbReader = new DBReader();
            string SQLQuery = "SELECT catagory, species, imgpath FROM pathtableorignal WHERE species < 6;";
            dbReader.orignalImgPathReader(SQLQuery, imgPath);
            WriteLine("Data Parsing from Orignal Path Table: done");
        }

        public void DataParseFromPreprocessedPathTable(List<PreprocessedPath.ImgPathPreprocessed> imgDataPreprocesseds, int process)
        {
            DBReader dbReader = new DBReader();
            string SQLQuery = $"SELECT catagory, species, preprocess, modifier, imgpath FROM pathtable WHERE preprocess = {process};";
            dbReader.preprocessedImgPathReader(SQLQuery, imgDataPreprocesseds);
            WriteLine("Data Parsing from Preprocessed Path Table: done");
        }
    }
}