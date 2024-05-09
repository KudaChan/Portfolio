from rembg import remove
from PIL import Image
from io import BytesIO

# import onnxruntime as ort
import numpy as np
import psycopg2
import os

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

    except Exception as e:
        print(f"Failed to connect to the database: {e}")

    finally:
        if cursor is not None:
            cursor.close()
        if conn is not None:
            conn.close()

def remove_bck(species_dict, img_data):
    action = "bgrndremove"
    for img in img_data:
        output_path = r"D:\Dataset\medai\PreProcessed"
        species = species_dict[img.species_id]
        if img.catagory_id == 1:
            cur_base_dir = os.path.join(output_path, "leaf")
        else:
            cur_base_dir = os.path.join(output_path, "plant")

        new_dir = os.path.join(cur_base_dir, species, action, "nomod")

        file_name = os.path.basename(img.img_path)

        if not os.path.exists(new_dir):
            os.makedirs(new_dir)

        input = Image.open(img.img_path)
        output_path = os.path.join(new_dir, file_name)
        output = remove(
            input,
            alpha_matting=True,
            alpha_matting_foreground_threshold=270,
            alpha_matting_background_threshold=20,
            alpha_matting_erode_size=11,
            post_process_mask=True,
        )

        output.save(output_path)

if __name__ == "__main__":
    img_data, species_dict = db_connector_and_retriver()
    remove_bck(species_dict, img_data)