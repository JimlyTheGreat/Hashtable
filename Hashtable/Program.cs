using System;
using System.IO;
using System.Collections.Generic;

namespace Hashtable
{
    class Program
    {
        static void Main(string[] args)
        {
            //This stores the raw data returned for every line contained in the Contacts text document.
            string[] allContactsRawData = File.ReadAllLines("Contacts.txt");

            //The array that will store all of the contact data after the Person object creation has been completed. It has a length that is equal to the total number of contacts returned from the text file.
            Person[] allContacts = new Person[allContactsRawData.Length];

            //Instance of the hashtable that has a length the same as the number of contacts.
            HashTable hashtable = new HashTable(allContactsRawData.Length);




            //Creates a new person for each one of the contacts in the text file, adds it to the all contacts array, and then it uses the Insert method on the previously created hashtable.
            for (int i = 0; i < allContactsRawData.Length; i++)
            {
                Person newPerson = new Person();
                newPerson.lastName = allContactsRawData[i].Split(" ")[0];
                newPerson.phoneNumber = allContactsRawData[i].Split(" ")[1];

                allContacts[i] = newPerson;
                hashtable.Insert(newPerson.lastName, newPerson.phoneNumber);
            }

            //Gets user search info
            Console.WriteLine("Please search for the last name of the contact you want the phone number for.");

            //Until the user inputs a correct search term, this will loop.
            while (true)
            {
                string userInput = Console.ReadLine();
                if (userInput != null && userInput != "")
                {
                    //Makes a value pair (A value and a key) and then it returns the value that matches.
                    ValuePair returnedSearch = (ValuePair)hashtable.Search(userInput);
                    //Writes the returned value.
                    if(returnedSearch != null)
                    {
                        Console.WriteLine(returnedSearch.value);
                    }
                    else
                    {
                        Console.WriteLine("Name not found");
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("Please enter a last name to search for.");
                }
            }
            Console.ReadLine();
        }
    }

    public class ValuePair
    {
        //Stores the key and value.
        public object key;
        public object value;

        //Requires both a key and value.
        public ValuePair(object Key, object Value)
        {
            key = Key;
            value = Value;
        }
    }

    //The class that stores the info for each person.
    class Person
    {
        public string lastName;
        public string phoneNumber;
    }

    //The nodes that will be used in the linked list.
    public class LLNode
    {
        public LLNode next;
        public object data;

        public LLNode(object Data)
        {
            SetData(Data);
            next = null;
        }

        public void SetData(object Data)
        {
            data = Data;
        }

        public void SetNext(LLNode newNode)
        {
            next = newNode;
        }

        public object GetData()
        {
            return data;
        }

        public LLNode GetNext()
        {
            return next;
        }


    }

    //The linked list class that will be used whenever a collision occurs.
    class LinkedList
    {
        public LLNode front;
        public LLNode back;
        public LLNode current;


        public void AddNewNode(object data)
        {
            LLNode newNode = new LLNode(data);

            if (front == null)
            {
                front = newNode;
                current = newNode;
                back = newNode;
            }
            else
            {
                current.SetNext(newNode);
                current = newNode;
                back = newNode;
            }
        }

        public void DisplayAllNodeValues()
        {
            current = front;
            while (current != null)
            {
                Console.WriteLine(current.data);
                current = current.next;
            }
        }

        public void SetFront(ValuePair data)
        {
            LLNode newNode = new LLNode(data);

            newNode.next = front.next;
            front = newNode;
        }

        public void SetBack(ValuePair data)
        {
            LLNode newNode = new LLNode(data);

            back = newNode;
        }

        public void SetCurrent(ValuePair data)
        {
            LLNode newNode = new LLNode(data);

            newNode.next = current.next;
            current = newNode;
        }

        public LLNode GetFront()
        {
            return front;
        }

        public LLNode GetBack()
        {
            return back;
        }

        public LLNode GetCurrent()
        {
            return current;
        }
    }

    public class HashTable
    {
        //Sets the size for the table.
        int size;
        //The array that stores the ValuePairs.
        object[] allValues;

        //Requires a size when instantiated
        public HashTable(int Size)
        {
            size = Size;
            //Sets the size of the array based on the input size.
            allValues = new object[size];
        }

        //The insert method that takes a key and value.
        public void Insert(object key, object value)
        {
            //The index location to insert the valuePair into.
            int indexInsertLocation = GetIndex(key);

            //Creates the new valuePair.
            ValuePair valuePair = new ValuePair(key, value);

            //Checks to see if the index in the array is null, if it is not null, a value is already there and we need to create a linked list.
            if (allValues[indexInsertLocation] != null)
            {
                //Checks to see if a linked list already exists. If a linked list is already there, then we insert the value into it. If there is no linked list, we make one and insert both values.
                string checkType = allValues[indexInsertLocation].GetType().Name;
                if (checkType != "LinkedList")
                {
                    LinkedList linkedList = new LinkedList();

                    //Add the current ValuePair to the new linkedList and add the new ValuePair.
                    linkedList.AddNewNode(allValues[indexInsertLocation]);
                    linkedList.AddNewNode(valuePair);

                    //Inserts the new linked list into the array.
                    allValues[indexInsertLocation] = linkedList;
                }
                //Adds the new value to the linked list.
                else
                {
                    LinkedList linkedList = new LinkedList();
                    linkedList = (LinkedList)allValues[indexInsertLocation];
                    linkedList.AddNewNode(valuePair);

                    allValues[indexInsertLocation] = linkedList;
                }
            }
            //Inserts the value pair as long as there isn't anything already in the array slot.
            else
            {
                allValues[indexInsertLocation] = valuePair;
            }
        }

        //Method that is used to search through the hashtable
        public object Search(object key)
        {
            //Get the index.
            int index = GetIndex(key);
            //Instantiate the object that will store the returned value.
            object objectFound = null;


            //Checks to see if there is a value at the index.
            if (allValues[index] != null)
            {
                //Value that is searched for.
                ValuePair valuePairToSearchFor;

                //Checks if the value is a linked list and if the key is the value searched for.
                if (allValues[index].GetType().Name != "LinkedList")
                {
                    valuePairToSearchFor = (ValuePair)allValues[index];
                    if(Convert.ToString(valuePairToSearchFor.key) == Convert.ToString(key))
                    {
                        objectFound = allValues[index];
                    }
                    else
                    {
                        objectFound = new ValuePair(key, "No object found");
                    }
                }
                //If it's a linked list, it goes through the linked list and finds the value searched for.
                else
                {
                    LinkedList foundList = (LinkedList)allValues[index];
                    foundList.current = foundList.front;

                    ValuePair currentValue = (ValuePair)foundList.current.data;

                    while (true)
                    {
                        if (Convert.ToString(currentValue.key) == Convert.ToString(key))
                        {
                            objectFound = currentValue;
                            break;
                        }
                        else if (currentValue == null)
                        {
                            objectFound = new ValuePair(key, "Name not found. Please search for a different name.");
                            break;
                        }
                        else
                        {
                            foundList.current = foundList.current.next;
                            if(foundList.current != null)
                            {
                                if (foundList.current.data != null)
                                {
                                    currentValue = (ValuePair)foundList.current.data;
                                }
                                else
                                {
                                    break;
                                }

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return objectFound;
        }

        //Returns the index to be used based off of the key given.
        private int GetIndex(object key)
        {
            int hashCode = key.GetHashCode();
            //Takes the remainder so we don't have to have an array that is millions of spaces long.
            int index = Math.Abs(hashCode % size);
            return index;
        }
    }
}
