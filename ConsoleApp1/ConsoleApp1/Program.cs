/* 
 * COIS 2020 - Data Structures and Algorithms, Trent Univerisity Fall 2018 
 * Assignment 2
 * Done by: Nikhil Pai Ganesh - 0595517 
 *          Anuj Arora - 0594437
 * Description: Use of Arrays, Hash table Dictionary and Binary Tree Algorithm to build a Huffman Tree which encodes and decodes the given string input.
 *              It is a compression Algorithm used to narrow down the memory size of the Input string in an encoded pattern. 
 *              However, Note that Huffman Tree is not an encryption algorithm. 
 *              Check the demonstartion of the Source Code in the documentation attached.
*/

using Huffman_Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huffman_Tree
{
    public class Node // Need to make this an interface (IComparable)
    {
        public char Character { get; set; }
        public int Frequency { get; set; }
        public Node Left { get; set; }//changed
        public Node Right { get; set; }
        

        public List<bool> Traverse(char character, List<bool> data)
        {
            /* 
             * this.Character = character;
             * this.Frequency = frequency; 
             * this.Right = right;
             * this.Left = left; 
             * this.Data = data;
             * 
             */

            // Insert a CompareTo Method Here and Explain the following Code in an object format 
            // Create an Array of Frequencies
            // Leaf
            if (Right == null && Left == null)
            {
                if (character.Equals(this.Character))
                {
                    return data;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                List<bool> left = null;
                List<bool> right = null;

                if (Left != null)
                {
                    List<bool> leftPath = new List<bool>();
                    leftPath.AddRange(data);
                    leftPath.Add(false);

                    left = Left.Traverse(character, leftPath);
                }

                if (Right != null)
                {
                    List<bool> rightPath = new List<bool>();
                    rightPath.AddRange(data);
                    rightPath.Add(true);
                    right = Right.Traverse(character, rightPath);
                }

                if (left != null)
                {
                    return left;
                }
                else
                {
                    return right;
                }
            }
        }
    }
}

// Class Huffman 
public class Huffman
{
    private List<Node> nodes = new List<Node>();
    public Node Root { get; set; }
    public Dictionary<char, int> // calling a dictonary 
                                 // Need to call Dictionary as D 

        Frequencies = new Dictionary<char, int>(); // frequency is referenced to the Dictionary 

    public void Build(string source) // String S instead of Source 
    {
        for (int i = 0; i < source.Length; i++)
        {
            if (!Frequencies.ContainsKey(source[i]))
            {
                Frequencies.Add(source[i], 0);
            }

            Frequencies[source[i]]++;
        }

        foreach (KeyValuePair<char, int> symbol in Frequencies)
        {
            nodes.Add(new Node() { Character = symbol.Key, Frequency = symbol.Value });
        }

        while (nodes.Count > 1)
        {
            List<Node> orderedNodes = nodes.OrderBy(node => node.Frequency).ToList<Node>();

            if (orderedNodes.Count >= 2)
            {
                // Take first two items
                List<Node> taken = orderedNodes.Take(2).ToList<Node>();

                // Create a parent node by combining the frequencies
                Node parent = new Node()
                {
                    Character = '*',
                    Frequency = taken[0].Frequency + taken[1].Frequency,
                    Left = taken[0],
                    Right = taken[1]
                };

                nodes.Remove(taken[0]);
                nodes.Remove(taken[1]);
                nodes.Add(parent);
            }

            this.Root = nodes.FirstOrDefault();

        }

    }

    //Encoding Method 
    public System.Collections.BitArray Encode(string source)
    {
        List<bool> encodedSource = new List<bool>();

        for (int i = 0; i < source.Length; i++)
        {
            List<bool> encodedSymbol = this.Root.Traverse(source[i], new List<bool>());
            encodedSource.AddRange(encodedSymbol);
        }

        BitArray bits = new BitArray(encodedSource.ToArray());

        return bits;
    }

    //Decoding Method
    public string Decode(BitArray bits)
    {
        Node current = this.Root;
        string decoded = "";

        foreach (bool bit in bits)
        {
            if (bit)
            {
                if (current.Right != null)
                {
                    current = current.Right;
                }
            }
            else
            {
                if (current.Left != null)
                {
                    current = current.Left;
                }
            }

            if (IsLeaf(current))
            {
                decoded += current.Character;
                current = this.Root;
            }
        }

        return decoded;
    }

    public bool IsLeaf(Node node)
    {
        return (node.Left == null && node.Right == null);
    }


    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Enter 1 for Decode & 2 for Encode Text");
            int option = Convert.ToInt32(Console.ReadLine());

            if (option == 1)
            {
                Console.Write(" Enter (Decoded) Input to Decode = >");
                string input = Console.ReadLine();
                Huffman huffmanTree = new Huffman();

                // Build the Huffman tree
                huffmanTree.Build(input);

                Console.WriteLine("***************************************************************************************************");

                // Encode
                BitArray encoded = huffmanTree.Encode(input); // This throws exception at the moment // Error: Cannot Implicitly Convert string to System.Collections.BitArray

                Console.Write("Encoded: ");
                foreach (bool bit in encoded)
                {
                    Console.Write((bit ? 1 : 0) + "");
                }
                Console.WriteLine();

                // Decode
                string decoded = huffmanTree.Decode(encoded);

                Console.WriteLine("Decoded: " + decoded);
                Console.WriteLine("**************************************************************************************************");
                Console.ReadLine();
            }
            else
            {
                Console.Write(" Enter (Encoded) Input to Decode = >");
                string inbut = Console.ReadLine();
                Huffman huffmanTree = new Huffman();

                // Build the Huffman tree
                huffmanTree.Build(inbut);

                Console.WriteLine("***************************************************************************");

                // Encode
                BitArray decoded = huffmanTree.Decode(inbut);
                // Problem is here
                // At the moment it throws an exception 

                Console.Write("Decoded: ");
                foreach (bool bit in decoded)
                {
                    Console.Write((bit ? 1 : 0) + "");
                }
                Console.WriteLine();

                // Decode
                string encoded = huffmanTree.Decode(decoded);

                Console.WriteLine("Encoded: " + encoded);
                Console.WriteLine("***************************************************************************");
                Console.ReadLine();
            }
        }
    }

    // this method is temporary (this needs to go as we figure out how to decode the encoding in the Main Method)
    private BitArray Decode(string inbut)
    {
        throw new NotImplementedException();
    }
}