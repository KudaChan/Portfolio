using Minor_Project_Ai_Plant_Recognition.SourceCode.DataStructure;

namespace Minor_Project_Ai_Plant_Recognition.SourceCode.DataBaseAction
{
    internal class DBMain
    {
        private readonly Dictionary<int, string> speciesDictIdx = new();
        private readonly Dictionary<int, string> processDictIdx = new();
        private readonly Dictionary<int, string> modifierDictIdx = new();

        public static void SpeciesDictInit(Dictionary<int, string> speciesDict)
        {
            _ = new DBReader();

            string speciesIDXQuery = "SELECT idx, sname FROM species_idx;";

            DBReader.IndexReader(speciesIDXQuery, speciesDict);

            WriteLine("SpeciesIdx to Dict: done");
        }

        public static void ProcessDictInit(Dictionary<int, string> processDict)
        {
            _ = new DBReader();

            string processIDXQuery = "SELECT idx, pname FROM process_idx;";

            DBReader.IndexReader(processIDXQuery, processDict);

            WriteLine("ProcessIdx to Dict: done");
        }

        public static void ModifierDictInit(Dictionary<int, string> modifierDict)
        {
            _ = new DBReader();

            string modifierIDXQuery = "SELECT idx, mname FROM modifier_idx;";

            DBReader.IndexReader(modifierIDXQuery, modifierDict);

            WriteLine("ModifierIdx to Dict: done");
        }

        public void DataParserFromOrignalDirAndFeeder()
        {
            SpeciesDictInit(speciesDictIdx);
            _ = new DBFeeder();
            string path = "D:\\Dataset\\medai\\DataOrignal";
            string[] files = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

            List<OrignalImgPath.ImgPathOrignal> imgData = [];

            foreach (string file in files)
            {
                string[] parts = file.Split('\\');

                string species = parts[^2];
                string catagory = parts[^3];

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

                OrignalImgPath.ImgPathOrignal imgPathOrignal = new()
                {
                    catagoryId = catagoryID,
                    speciesId = speciesID,
                    imgPath = file
                };

                imgData.Add(imgPathOrignal);
            }

            DBFeeder.DataFeederToOrignalPathtable(imgData);

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

            _ = new
            DBFeeder();
            string path = "D:\\Dataset\\medai\\PreProcessed";
            string[] files = Directory.GetFiles(path, "*.jpg", SearchOption.AllDirectories);

            List<PreprocessedPath.ImgPathPreprocessed> imgData = new();

            foreach (string file in files)
            {
                string[] parts = file.Split('\\');

                string species = parts[^4];
                string catagory = parts[^5];
                string process = parts[^3];
                string modifier = parts[^2];

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

                PreprocessedPath.ImgPathPreprocessed imgPathPreprocessed = new()
                {
                    catagoryId = catagoryID,
                    speciesId = speciesID,
                    preprocessedId = processID,
                    modifierId = modifierID,
                    imgPath = file
                };

                imgData.Add(imgPathPreprocessed);
            }

            DBFeeder.DataFeederToPathtable(imgData);

            WriteLine("Total img count" + imgData.Count);

            WriteLine("Data Feeding to Preprocessed Path Table: done");
        }

        public static void DataParseFromOrignalPathTable(List<OrignalImgPath.ImgPathOrignal> imgPath)
        {
            _ = new DBReader();
            string SQLQuery = "SELECT catagory, species, imgpath FROM pathtableorignal WHERE species < 6;";
            DBReader.OrignalImgPathReader(SQLQuery, imgPath);
            WriteLine("Data Parsing from Orignal Path Table: done");
        }

        public static void DataParseFromPreprocessedPathTable(List<PreprocessedPath.ImgPathPreprocessed> imgDataPreprocessed, int process)
        {
            _ = new DBReader();
            string SQLQuery = $"SELECT catagory, species, preprocess, modifier, imgpath FROM pathtable WHERE preprocess = {process};";
            DBReader.PreprocessedImgPathReader(SQLQuery, imgDataPreprocessed);
            WriteLine("Data Parsing from Preprocessed Path Table: done");
        }
    }
}