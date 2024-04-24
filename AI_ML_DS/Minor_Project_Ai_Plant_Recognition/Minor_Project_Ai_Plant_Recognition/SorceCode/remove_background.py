from rembg import remove
from PIL import Image
from io import BytesIO
import numpy as np
from concurrent.futures import ThreadPoolExecutor
import shutil
import cv2
import os

output_path = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Data\\Dataset(background_removed)"

text_file_dir = "D:\\Project\\AI_ML_DS\\Minor_Project_Ai_Plant_Recognition\\Minor_Project_Ai_Plant_Recognition\\Temp\\dataset"

def remove_background_factory():
    """
    This function is the main driver for removing the background from images.
    It first constructs the paths to the text files containing the paths of the images to process.
    It then checks if these files exist and if they are not empty.
    If the files exist and are not empty, it reads the image paths from the files and starts a ThreadPoolExecutor.
    The ThreadPoolExecutor maps the process_image function to the image paths, effectively starting a new thread for each image.
    If an exception occurs during the processing of an image, it is caught and logged to the console.
    """
    path224 = os.path.join(text_file_dir, "dataset_size224_224.txt")
    path299 = os.path.join(text_file_dir, "dataset_size299_299.txt")

    path_list = [path224, path299]

    for path in path_list:
        if not os.path.exists(path):
            print(f"File does not exist: {path}")
            continue

        try:
            with open(path, "r") as f:
                image_paths = f.readlines()
                if not image_paths:
                    print(f"File is empty: {path}")
                    continue

                with ThreadPoolExecutor() as executor:
                    executor.map(process_image, image_paths)
        except Exception as e:
            print(f"An error occurred: {e}")

def process_image(image_path):
    """
    This function processes a single image by removing its background.
    It first strips the newline characters from the image path.
    It then calls the remove_backgrnd function with the cleaned image path.
    """
    image_path = image_path.strip()  # remove newline characters
    remove_backgrnd(image_path)

def remove_backgrnd(image_path):
    """
    This function removes the background from the image at the specified path.
    It first opens the image file in binary mode and reads its content.
    It then calls the remove function from the rembg library to remove the background from the image.
    The removed background image is then converted to an RGB image using the PIL library.
    The img_writer_assistance function is then called with the RGB image and the image path.
    """
    with open(image_path, "rb") as input_img_file:
        input_img = input_img_file.read()
        back_remove = remove(input_img, alpha_matting_background_threshold=200)

        img = Image.open(BytesIO(back_remove)).convert("RGB")

        img_writer_assistance(img, image_path)

def img_writer_assistance(img, img_path):
    """
    This function assists in writing the processed image to a specific output directory.
    It first gets the parent directories of the original image and constructs a specific output directory path.
    The output directory path is constructed by joining the output_path and the names of the parent directories.
    The image name is extracted from the original path.
    The img_writer function is then called with the specific output directory, the processed image, and the image name.
    """
    dir_parent_parent_parent_info = os.path.dirname(
        os.path.dirname(os.path.dirname(os.path.dirname(img_path)))
    )
    dir_parent_parent_info = os.path.dirname(os.path.dirname(os.path.dirname(img_path)))
    dir_parent_info = os.path.dirname(os.path.dirname(img_path))
    dir_info = os.path.dirname(img_path)
    dir_name = os.path.basename(dir_info)
    dir_parent_name = os.path.basename(dir_parent_info)
    dir_parent_parent_name = os.path.basename(dir_parent_parent_info)
    dir_parent_parent_parent_name = os.path.basename(dir_parent_parent_parent_info)
    img_name = os.path.basename(img_path)
    specific_output_directory = os.path.join(
        output_path,
        dir_parent_parent_parent_name,
        dir_parent_parent_name,
        dir_parent_name,
        dir_name,
    )

    img_writer(specific_output_directory, img, img_name)

def img_writer(dirpath, new_img, img_name):
    """
    This function writes the processed image to the specified directory.
    It first checks if the directory exists. If it does, it removes the directory and its contents.
    It then creates the directory.
    If an exception occurs during the directory creation, it is caught and logged to the console.
    The function then constructs the path to the new image file by joining the directory path and the image name.
    It then tries to write the image to the new path.
    If an IOError occurs during the image writing, it is caught and logged to the console.
    """
    path = dirpath
    if os.path.exists(path):
        shutil.rmtree(path)
        try:
            os.makedirs(path)
        except Exception as e:
            print(f"Failed to create directory: {e}")
    else:
        try:
            os.makedirs(path)
        except Exception as e:
            print(f"Failed to create directory: {e}")

    new_path = os.path.join(path, img_name)

    try:
        new_img_array = np.array(new_img)
        cv2.imwrite(new_path, new_img_array)
    except IOError as e:
        print("In NewImageWriter")
        print(f"Failed to write image: {e}")

if __name__ == "__main__":
    """
    This is the entry point of the script.
    It calls the remove_background_factory function which is responsible for removing the background from images.
    The function reads the paths of the images from text files and starts a new thread for each image.
    """
    remove_background_factory()