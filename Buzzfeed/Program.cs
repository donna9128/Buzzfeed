using System;
using System.Data.SqlClient;

namespace Buzzfeed
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlConnection connection = new SqlConnection(@"Server=minecraft.lfgpgh.com;Database=buzzfeed01;User Id=academy_student;Password=12345");

            connection.Open();

            //design a program that allows a user to make a quiz
            //ask for the name of the quiz
            //provide a quit option
            //create a loop for multiple questions/answers/values
            //ask for questions and applicable answers
            //create loop for results
            //ask for results
            //provide option for the user to see the progress of what they've created

            //Bonus:
            //Edit existing quiz-Sql command update
            //Restart/reset option
            //display progress at any time

            bool quiz = true;

            while (quiz)
            {
                Console.WriteLine("Welcome to the quiz generator! Let's make a quiz!");

                Console.WriteLine("Would you like to:\n (a) Make a new quiz \n (b) quit the program");
                string quizResponse = Console.ReadLine().ToLower();

                Console.WriteLine("You'll be asked to set up your questions and answers first, and then the results afterward. \nBe sure to have in mind how many Q's and A's you want in your quiz, I'll be asking about that.");

                if (quizResponse == "a")
                {

                    //get quiz name and establish variable
                    Console.WriteLine("Tell us the name of your quiz");
                    string quizname = Console.ReadLine();

                    //insert the quiz name into the quizzes table
                    SqlCommand command = new SqlCommand($"INSERT INTO Quizzes (Title) VALUES ('{quizname}')", connection);
                    command.ExecuteNonQuery();

                    //select quiz titles and matching it to the quiz name for this user. then making quizzesid variable to be used later
                    SqlCommand command1 = new SqlCommand($"SELECT * FROM Quizzes WHERE Title = ('{quizname}')", connection);
                    SqlDataReader reader = command1.ExecuteReader();
                    //reading ID in quizzes so that we can have a quiz id in questions
                    int quizzesid = 0;
                    while (reader.Read())
                    {
                        quizzesid = Convert.ToInt32(reader["Id"]);
                    }
                    reader.Close();

                    //establish a loop for multiple questions
                    int thisMany = 0;
                    bool questions = true;
                    while (questions)
                    {
                        Console.WriteLine("Would you like to add a question? \na)yes \nb)no");
                        string response = Console.ReadLine().ToLower();


                        if (response == "a")
                        {

                            Console.WriteLine("How many questions are you going to make? Please provide a number");
                            int howMany = Convert.ToInt32(Console.ReadLine());

                            for (int i = 1; i <= howMany; i++)
                            {
                                //establishing variables within if loop so they reset with every question
                                string question = "";
                                Console.WriteLine("What is the text of your question?");
                                question = Console.ReadLine();
                                //quiz id cooresponds to lines 43-46
                                //inserting data into questions table using variables quizzesid and question
                                SqlCommand questionCommand = new SqlCommand($"INSERT INTO Questions (QuizId, Text) VALUES ({quizzesid}, '{question}')", connection);
                                questionCommand.ExecuteNonQuery();
                                //cooresponding question data with variable
                                SqlCommand questioncommand2 = new SqlCommand($"SELECT * FROM Questions WHERE Text = ('{question}')", connection);
                                //using reader to find data
                                SqlDataReader questionreader = questioncommand2.ExecuteReader();

                                int questionid = 0;
                                while (questionreader.Read())
                                {
                                    questionid = Convert.ToInt32(questionreader["Id"]);
                                }

                                questionreader.Close();

                                Console.WriteLine("How many answers will this question have? Please provide a number");
                                thisMany = Convert.ToInt32(Console.ReadLine());

                                //y is used as value
                                for (int y = 1; y <= thisMany; y++)
                                {
                                    string answer = "";
                                    Console.WriteLine("What is the text of your answer?");
                                    answer = Console.ReadLine();
                                    //instering questionid, value and answer to cooresponding columsn in answers table
                                    SqlCommand answerCommand = new SqlCommand($"INSERT INTO Answers (QuestionId, Value, Text) VALUES ({questionid}, {y}, '{answer}')", connection);
                                    answerCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        if (response == "b")
                        {
                            questions = false;
                        }

                    }
                    bool results = true;
                    while (results)
                    {
                        //need quizid, score, title, text
                        // This loop setup is different from the questions and answers, allowing the user to keep adding results as much as they want without having to declare how many before hand
                        // Only had to do that before because it solved the "score value" portion of the answers table
                        Console.WriteLine("Would you like to add more quiz results? a) yes b) no");
                        string response = Console.ReadLine().ToLower();

                        if (response == "a")
                        {
                            //How would we tackle this
                            //let the user decide the range of scoring to use to provide different results
                            //let the user decide how many results they're going to give
                            //this way range and number of results will be independent from variable established earlier in code

                            // Prompt the user what info they'll need to make their results section
                            Console.WriteLine("We're gonna need some info from you. Be sure to have in mind the total range of scores for your quiz:\nthe name of your result, some text describing it, and the score where you'd like to place the result in your total range.");

                            // Get the Title Column of the result
                            Console.WriteLine("First, the name of the result:");
                            string resultName = Console.ReadLine();

                            // Get the Text Columnn of the result
                            Console.WriteLine("Next is any text you'd like to add describing the result:");
                            string flavorText = Console.ReadLine();

                            // Ask the user to set up where in their hypothetical total score range where they'd like the result to land
                            // The scores will be handled from that number and above when someone is taking the quiz in order to give them their result
                            Console.WriteLine("Lastly the number within your total score range where you'd like this result to exist:");
                            int resultScore = Convert.ToInt32(Console.ReadLine());

                            // SQL command inserting the information we've just received into the Results Table
                            // Still using the same quizzesid variable because we're within the Quiz while loop
                            SqlCommand resultCommand = new SqlCommand($"INSERT INTO Results (QuizId, Score, Title, Text) VALUES ({quizzesid}, {resultScore}, '{resultName}', '{flavorText}')", connection);
                            resultCommand.ExecuteNonQuery();
                        }

                        // Separate response to leave the results loop
                        if (response == "b")
                        {
                            results = false;
                        }
                    }
                }

                // response to leave the making a quiz program
                if (quizResponse == "b")
                {
                    quiz = false;
                }
                connection.Close();
            }
        }
    }
}

//started potential code for the results section

//int range = thisMany * 4;
//Console.WriteLine("Your score range is: " + range);
//for (int i = 0; i <= range; i = i + 4)
//Console.WriteLine("What is the title of the result?");
//string resulttitle = Console.ReadLine();
//Console.WriteLine("Please provide a commentary of this result.");
//string resulttext = Console.ReadLine();
