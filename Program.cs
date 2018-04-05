/* Program.cs
 * Fatih Yılmaz
 * This program is written for piworks internship assignment.
 * Program reads some data from a csv file and produces desired output to another csv file. 
*/

using System;
using System.IO;
using System.Collections.Generic;

namespace piworks
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// Data is extracted from file named exhibitA-input.csv. It is assumed to be in the same directory with the exe file.
			using(var reader = new StreamReader("exhibitA-input.csv"))
			{
				List<int> songId = new List<int>(); // Holds every song id of csv file
				List<int> clientId = new List<int>(); // Holds every client id of csv file

				System.Console.WriteLine ("Reading file...");

				// Read while it is not the end of the file
				while (!reader.EndOfStream)
				{
					var line = reader.ReadLine();
					// Add to list if date is 10/08/2016
					if (line.Contains ("10/08/2016")) {
						var values = line.Split('	'); // Split line elements by tabs
						songId.Add(Int32.Parse(values[1])); // Add second element to songId list
						clientId.Add(Int32.Parse(values[2])); // Add third element to songId list
					}
				}

				// Finding max client id
				int maxClient = 0; // Holds max client id
				foreach (int client in clientId) {
					if (client > maxClient) {
						maxClient = client;
					}
				}

				List<int> NumberOfUsers = new List<int>(); // Holds users quantity for last result
				List<int> NumberOfSongs = new List<int>(); // Holds distinct songs play for last result

				// Feedback message for console
				System.Console.WriteLine ("Calculating played distinct songs and corresponding number of clients...");

				// Searches every row on csv file for every client. Evaluates if client id matches with index number.  
				for (int i = 1; i <= maxClient; i++) {

					List<int> tempSongList = new List<int>(); // Holds client's listened song list temporary for this iteration. 
					int songIndex = 0; // Indexing for tempSongList list.
					bool exists = false; // Checker variable whether client with id i exists

					// Check every client id 
					foreach (int client in clientId) {
						// If it matches with for loop's index(variable i)
						if (client == i) {
							// Also check if  corresponding song id is already on the tempSongList
							if(tempSongList.Find(x => x == songId[songIndex] ) == 0) {
								// If it is not already inside that list add song id to list. 
								tempSongList.Add(songId[songIndex]);
							}
							exists = true; // If client with id i is found assign variable to true
						}
						songIndex++;
					}

					// If client with id i exists
					if (exists) {
						// Check if this clients distinctly played number of songs is already in the list
						// If it is in the list gets index on NumberOfSongs list for that number. If not returns -1 
						int index = NumberOfSongs.FindIndex(x => x == tempSongList.Count ); 
						// If that number is not on list 
						if (index == -1) {
							NumberOfSongs.Add (tempSongList.Count); // Add that number to list 
							NumberOfUsers.Add (1); //  Also add client for corresponding number of songs
						} else {
							// If played song number is on the list do not add that number to list
							// But increase the number of users for that played song number
							NumberOfUsers [index]++; 
						}
					}


				} // End of for


				// Feedback message for console
				System.Console.WriteLine ("Sorting final data...");

				// Bubble Sort
				for(int i=0; i < NumberOfSongs.Count; i++){
					for (int j = 0; j < NumberOfSongs.Count - i - 1; j++) {
						if (NumberOfSongs [j] > NumberOfSongs [j + 1]) {
							int temp = NumberOfSongs [j];
							int temp2 = NumberOfUsers [j];

							NumberOfSongs [j] = NumberOfSongs [j + 1];
							NumberOfSongs [j + 1] = temp;

							NumberOfUsers [j] = NumberOfUsers [j + 1];
							NumberOfUsers [j + 1] = temp2;

						}
					}
				}

				// Feedback message for console
				System.Console.WriteLine ("Extracting to file...");

				int outputIndex = 0; // Index for both NumberOfUsers and NumberOfSongs lists
				// Open a new file stream. Write to a file named fastafaryan-output.csv
				using (System.IO.StreamWriter file = new System.IO.StreamWriter("fastafaryan-output.csv"))
				{
					// Write header for file
					file.Write("DISTINCT_PLAY_COUNT");
					file.Write(","); // Separate columns with comma
					file.Write("CLIENT_COUNT"); 
					file.Write("\n"); // Add new line character

					// Write every NumberOfUsers and NumberOfSongs data line by line
					foreach (int song in NumberOfSongs)
					{
						file.Write(song);
						file.Write(","); // Separate columns with comma
						file.Write(NumberOfUsers[outputIndex]);
						file.Write("\n"); // Add new line character
						outputIndex++;
					}
				}

				// Feedback message for console
				System.Console.WriteLine ("File named 'fastafaryan-output.csv' is extracted to the same directory with your exe file.");


			}
		}
	}
}
