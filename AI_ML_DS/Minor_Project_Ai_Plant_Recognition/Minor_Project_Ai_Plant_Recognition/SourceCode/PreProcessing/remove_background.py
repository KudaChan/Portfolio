import os
import warnings
from concurrent.futures import ThreadPoolExecutor

import psycopg2
from PIL import Image
from rembg.bg import new_session, remove

warnings.filterwarnings(
    "ignore", category=UserWarning, module="onnxruntime.capi.onnxruntime_validation"
)


class ImgPathOrignal:
    def __init__(self, img_path, species_id, catagory_id):
        self.img_path = img_path
        self.species_id = species_id
        self.catagory_id = catagory_id


def db_connector_and_retriver():
    """
    This function connects to the database and returns the connection object.
    It first constructs the connection string using the credentials and the database name.
    It then tries to connect to the database using the connection string.
    If an exception occurs during the connection, it is caught and logged to the console.
    """

    img_data = []
    species_dict = {}

    try:
        conn = psycopg2.connect(
            dbname=os.getenv("DB_NAME"),
            user=os.getenv("DB_USER"),
            password=os.getenv("DB_PASSWORD"),
            host=os.getenv("DB_HOST"),
            port=os.getenv("DB_PORT"),
        )

        cursor = conn.cursor()

        print("Connected to the database")

        idx_query = "SELECT * FROM species_idx"
        cursor.execute(idx_query)
        rows = cursor.fetchall()

        species_dict = {row[0]: row[1] for row in rows}

        img_query = (
            "SELECT catagory, species, imgpath FROM pathtableorignal WHERE species < 6;"
        )
        cursor.execute(img_query)
        rows = cursor.fetchall()

        for row in rows:
            img_data.append(ImgPathOrignal(row[2], row[1], row[0]))

        return img_data, species_dict

    except psycopg2.Error as e:
        print(f"Failed to connect to the database: {e}")

    finally:
        if cursor is not None:
            cursor.close()
        if conn is not None:
            conn.close()
            print("Database connection closed.")


def process_image(img, species_dict, output_path, action):

    print("Processing image: ", img.img_path)

    species = species_dict[img.species_id]
    if img.catagory_id == 1:
        cur_base_dir = os.path.join(output_path, "leaf")
    else:
        cur_base_dir = os.path.join(output_path, "plant")

    new_dir = os.path.join(cur_base_dir, species, action, "nomod")

    file_name = os.path.basename(img.img_path)

    if not os.path.exists(new_dir):
        os.makedirs(new_dir)

    input_img = Image.open(img.img_path)
    input.load()
    output_path = os.path.join(new_dir, file_name)

    try:
        output = remove(
            input_img,
            alpha_matting=True,
            alpha_matting_foreground_threshold=500,
            alpha_matting_background_threshold=50,
            alpha_matting_erode_size=15,
            post_process_mask=True,
            session=new_session("isnet"),
        )
    except Exception as e:
        print(f"Failed to process image {file_name}: {e}")
        return

    output = output.convert("RGB")
    output.save(output_path)


def remove_bck(species_dict, img_data):
    action = "bgrndremove"
    output_path = r"D:\Dataset\medai\PreProcessed"

    with ThreadPoolExecutor(os.cpu_count()) as executor:
        executor.map(
            process_image,
            img_data,
            [species_dict] * len(img_data),
            [output_path] * len(img_data),
            [action] * len(img_data),
        )


if __name__ == "__main__":
    img_dataset, species_dict = db_connector_and_retriver()
    remove_bck(species_dict, img_dataset)
