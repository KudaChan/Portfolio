"""_summary_
imports
"""

import os
import random
import psycopg2
import matplotlib.pyplot as plt


class ImgData:
    """
    Initialize a new instance of ImgData.

    Parameters:
    img_path (str): The path to the image file.
    species_id (int): The unique identifier for the species of the image.
    catagory_id (int): The unique identifier for the catagory of the image.

    Returns:
    None
    """

    def __init__(self, img_path, species_id, catagory_id):
        self.img_path = img_path
        self.species_id = species_id
        self.catagory_id = catagory_id


class DBConnector:
    """
    This class is responsible for establishing connection to the database.
    """

    def __init__(self):
        """
        Initialize a new instance of db_connector.

        This method establishes a connection to a PostgreSQL database using the provided
        credentials.
        If the connection is successful, a cursor object is created for executing SQL queries.

        Parameters:
        None

        Returns:
        None

        Raises:
        psycopg2.Error: If the connection to the database fails.

        Note:
        The connection details are currently hardcoded for demonstration purposes.
        In a production environment, it is recommended to use environment variables or
        secure configuration files to store sensitive information.
        """
        try:
            # self.conn = psycopg2.connect(
            #     dbname=os.getenv("DB_NAME"),
            #     user=os.getenv("DB_USER"),
            #     password=os.getenv("DB_PASSWORD"),
            #     host=os.getenv("DB_HOST"),
            #     port=os.getenv("DB_PORT"),
            # )

            self.conn = psycopg2.connect(
                dbname="medai",
                user="kumar",
                password="chan05dan",
                host="localhost",
                port="5432",
            )

            self.cursor = self.conn.cursor()
            print("Connected to the database")
        except psycopg2.Error as e:
            print(f"Failed to connect to the database: {e}")

    def close_db(self):
        """
        This method closes the database connection and cursor.

        Parameters:
        None

        Returns:
        None

        Raises:
        None

        Note:
        This method should be called when the database operations are completed to
        free up resources.
        """
        if self.cursor is not None:
            self.cursor.close()
        if self.conn is not None:
            self.conn.close()
            print("Database connection closed.")

    def execute_sql(self, sql_query, params=None):
        """
        Executes a SQL query using the provided parameters and returns all rows fetched.

        Parameters:
        sql_query (str): The SQL query to be executed.
        params (tuple, optional): The parameters to be used in the SQL query. Defaults to None.

        Returns:
        list: A list of tuples, where each tuple represents a row fetched from the database.

        Raises:
        psycopg2.Error: If an error occurs while executing the SQL query.
        """
        self.cursor.execute(sql_query, params)
        rows = self.cursor.fetchall()
        return rows

    def data_loader(self, sql_query):
        """
        This method retrieves image data and species dictionary from the database.

        Parameters:
        sql_query (str): The SQL query to fetch image data from the database.

        Returns:
        tuple: A tuple containing two lists: img_data_list and species_dict.
            img_data_list is a list of ImgData instances representing the image data.
            species_dict is a dictionary mapping species IDs to their corresponding names.

        Note:
        This method first fetches species IDs and names from the 'species_idx' table
        using a separate SQL query.
        Then, it executes the provided SQL query to fetch image data.
        For each row fetched from the database, it creates an instance of ImgData and appends
        it to the img_data_list.
        Finally, it returns the img_data_list and species_dict.
        """
        img_data_list = []
        species_dict = {}

        idx_query = "SELECT * FROM species_idx WHERE idx <= 5;"
        rows = self.execute_sql(idx_query)
        species_dict = {row[0]: row[1] for row in rows}

        rows = self.execute_sql(sql_query)
        for row in rows:
            img_data_list.append(ImgData(row[2], row[1], row[0]))

        return img_data_list, species_dict


class DataSplitter:
    """_summary_
    This class split data into test and train.
    """

    def __init__(self, img_data, species_dict):
        """
        Initialize a new instance of DataSplitter.

        Parameters:
        img_data (list): A list of ImgData instances representing the image data.
        species_dict (dict): A dictionary mapping species IDs to their corresponding names.

        Returns:
        None
        """
        self.img_data = img_data
        self.species_dict = species_dict

    def split_data(self):
        """
        This method splits the image data into training and testing sets based on species.

        Parameters:
        self (DataSplitter): The instance of the DataSplitter class.

        Returns:
        tuple: A tuple containing two lists: train_data and test_data.
            train_data is a list of ImgData instances for training.
            test_data is a list of ImgData instances for testing.

        Note:
        The method iterates over each species in the species_dict, creates a temporary
        list of ImgData instances
        for that species, shuffles the list, and then splits it into training and testing
        sets based on a 80/20 ratio.
        The train_data and test_data lists are then extended with the corresponding
        data from the temporary list.
        """
        train_data = []
        test_data = []

        for species_id in self.species_dict.keys():
            temp_list = [
                img for img in self.img_data if img.species_id == species_id]

            random.shuffle(temp_list)
            split_index = int(len(temp_list) * 0.8)

            train_data.extend(temp_list[:split_index])
            test_data.extend(temp_list[split_index:])

        return train_data, test_data


class Main:
    """_summary_
    This class is starting point and called from different script.
    """

    def __init__(self):
        """
        Initialize a new instance of the main class.

        Parameters:
        process (int): The process identifier. It determines the type of data to be processed.

        Returns:
        None
        """
        print("Starting DataPreparation")
        self.species_dict = None

    def data_split_per_process(self, sql_query):
        """
        This method is responsible for splitting the data into training and testing sets
        based on the given SQL query.

        Parameters:
        self (main): The instance of the main class.
        sqlQuery (str): The SQL query to retrieve the data from the database.

        Returns:
        tuple: A tuple containing the train_data and test_data.

        Note:
        This method uses the DBConnector class to establish a connection to the database,
        retrieves the data using the provided SQL query, and then splits the data
        into training and testing sets.
        The species_dict is also generated from the retrieved data.
        The method prints the count of train and test data, as well as
        the species dictionary.
        Finally, it closes the database connection using the close_db method
        of the DBConnector class.
        """
        db = DBConnector()
        img_data, species_dict = db.data_loader(sql_query)
        db.close_db()
        data_splitter = DataSplitter(img_data, species_dict)
        train_data, test_data = data_splitter.split_data()
        print(f"    Train data count: {len(train_data)}")
        print(f"    Test data count: {len(test_data)}")
        print(f"    Species dict: {species_dict}")
        self.species_dict = species_dict

        return train_data, test_data

    def data_prepare_starter(self, process):
        """
        This method processes the data based on the given process.

        Parameters:
        self (main): The instance of the main class.
        process (int): The process identifier.

        Returns:
        tuple: A tuple containing the train_data and test_data. If the process is invalid,
        returns None, None.

        Note:
        The method prints the type of data being processed and calls the data_split_per_process
        method to split the data into training and testing sets.
        """
        if process == 0:
            pname = "Orignal Data"
            print("Orignal Data :")
            sql_query = "SELECT catagory, species, imgpath FROM pathtableorignal WHERE catagory = 1 AND species <= 5"

            train_data, test_data = self.data_split_per_process(sql_query)
        elif process == 1:
            pname = "Augmented Data"
            print("Augmented Data:")
            sql_query = "SELECT catagory, species, imgpath FROM pathtable where preprocess = 1 AND species < 3 AND catagory = 1"

            train_data, test_data = self.data_split_per_process(sql_query)
        elif process == 2:
            pname = "Background Removed Data(orignal)"
            print("Background Removed Data(orignal) :")
            sql_query = (
                "SELECT catagory, species, imgpath FROM pathtable where preprocess = 2"
            )

            train_data, test_data = self.data_split_per_process(sql_query)
        elif process == 3:
            pname = "Background Removed Data(augmented)"
            print("Background Removed Data(augmented) :")
            sql_query = (
                "SELECT catagory, species, imgpath FROM pathtable where preprocess = 3"
            )

            train_data, test_data = self.data_split_per_process(sql_query)
        elif process == 4:
            pname = "All Data Together"
            print("All Data Together :")
            sql_query = "SELECT catagory, species, imgpath FROM pathtable"

            train_data, test_data = self.data_split_per_process(sql_query)
        else:
            print("Invalid process.")
            return None, None, None

        print("Data preparation complete.")
        return train_data, test_data, pname

    def visualize_data(self, img_train_data_df, img_test_data_df):
        """
        This method visualizes the distribution of categories and species in the data.

        Parameters:
        img_data_df (DataFrame): The DataFrame containing the image data.
        species_dict (dict): A dictionary mapping species IDs to their corresponding names.

        Returns:
        None
        """
        img_total_data_df = pd.concat([img_train_data_df, img_test_data_df])
        img_total_data_df['species_id'] = img_total_data_df['species_id'].replace(
            self.species_dict)

        # Replace species IDs with their names for better visualization
        img_train_data_df['species_id'] = img_train_data_df['species_id'].replace(
            self.species_dict)

        img_test_data_df['species_id'] = img_test_data_df['species_id'].replace(
            self.species_dict)

        # Calculate the counts of each species in the training, testing, and total data
        train_counts = img_train_data_df['species_id'].value_counts()
        test_counts = img_test_data_df['species_id'].value_counts()
        total_counts = img_total_data_df['species_id'].value_counts()

        # Create a DataFrame that contains the counts
        counts_df = pd.DataFrame({
            'Train': train_counts,
            'Test': test_counts,
            'Total': total_counts
        })

        # Fill NaN values with 0
        counts_df = counts_df.fillna(0)

        # Convert the counts to integers
        counts_df = counts_df.astype(int)

        # Create a new figure
        fig, ax = plt.subplots(1, 1, figsize=(10, 6))

        # Hide axes
        ax.axis('off')

        # Create a table and add it to the figure
        table = plt.table(cellText=counts_df.values,
                          colLabels=counts_df.columns,
                          rowLabels=counts_df.index,
                          cellLoc='center',
                          loc='center',
                          bbox=[0, 0, 1, 1])  # Use bbox to position the table

        table.auto_set_font_size(True)
        # table.set_fontsize(10)  # Adjust font size
        table.scale(1, 1.5)  # Adjust table size

        # Set color for the cells in the table
        table_props = table.properties()
        table_cells = table_props['children']
        for cell in table_cells:
            cell.set_edgecolor('black')  # Set cell border color
            if cell.get_text().get_text() == 'Species' or cell.get_text().get_text() == 'Train' or cell.get_text().get_text() == 'Test' or cell.get_text().get_text() == 'Total':
                # Set cell background color for header
                cell.set_facecolor('skyblue')
            else:
                # Set cell background color for others
                cell.set_facecolor('white')

        plt.title('Data distribution table', fontsize=16, fontweight='bold')
        plt.show()

        # Plot the distribution of species
        plt.figure(figsize=(8, 6))
        total_counts = img_total_data_df['species_id'].value_counts()
        plt.bar(total_counts.index, total_counts.values,
                alpha=0.6, label='Total Data')

        # Plot the distribution of species in the testing data
        test_counts = img_test_data_df['species_id'].value_counts()
        plt.bar(test_counts.index, test_counts.values,
                alpha=0.6, label='Test Data')

        # Add labels and title
        plt.xlabel('Species Name')
        plt.ylabel('Count')
        plt.title('Distribution of Species in Total and Test Data')

        # Add a legend
        plt.legend()

        # Show the plot
        plt.show()


# if __name__ == '__main__':
#     import pandas as pd

#     # Initialize the Main class
#     main = Main()

#     # Prepare the data
#     train_data, test_data, pname = main.data_prepare_starter(
#         process=0)  # change the process number as per your requirement

#     # Convert the list of ImgData instances to a DataFrame
#     img_train_data_df = pd.DataFrame([(img.img_path, img.species_id, img.catagory_id) for img in train_data],
#                                      columns=['img_path', 'species_id', 'category_id'])
#     img_test_data_df = pd.DataFrame([(img.img_path, img.species_id, img.catagory_id) for img in test_data],
#                                     columns=['img_path', 'species_id', 'category_id'])

#     # Visualize the data
#     main.visualize_data(img_train_data_df, img_test_data_df)
