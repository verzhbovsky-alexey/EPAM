﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Task3EnumsLib.Enums;
using Task3FiguresLib.Figures;

namespace Task3FiguresLib
{
    public class Box
    {
        private static List<Figure> figures = new List<Figure>();

        public static void AddFigure(Figure figure)
        {
            if(!figures.Contains(figure))
            {
                if (figures.Count < 20)
                {
                    figures.Add(figure);
                }
            }
        }
        public static Figure ShowByNumber(int number)
        {
            try
            {
                return figures[number - 1];
            }
            catch(IndexOutOfRangeException)
            {
                return null;
            }
        }

        public static Figure ShowByNumberAndRemove(int number)
        {
            try
            {
                Figure figure = figures[number - 1];
                figures.RemoveAt(number - 1);
                return figure;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
        }

        public static void ReplaceByNumber(Figure figure, int number)
        {
            try
            {
                figures[number - 1] = figure;
            }
            catch (IndexOutOfRangeException)
            {
                
            }
        }

        public static List<Figure> FindEquivalent(Figure figure, int number)
        {
            List<Figure> sameFigures = figures.Where(o => o.Color.Equals(figure.Color) && o.Material.Equals(figure.Material)).ToList();
            if(sameFigures.Count > 0)
            {
                return sameFigures;
            }
            else
            {
                throw new Exception("AAA");
            }
        }

        public static int ShowQuantity()
        {
            return figures.Count;
        }

        public static double ShowSquareSum()
        {
            double sum = figures.Sum(o => o.GetSquare());
            return sum;
        }

        public static double ShowPrimetrSum()
        {
            double sum = figures.Sum(o => o.GetPerimetr());
            return sum;
        }

        public static List<Figure> ShowCircles()
        {
            List<Figure> circles = figures.Where(o => o is Circle).ToList();
            return circles;
        }

        public static List<Figure> ShowEnvelopeFigures()
        {
            List<Figure> circles = figures.Where(o => o.Material == Materials.Envelope).ToList();
            return circles;
        }

        public static void SaveFiguresByXmlWriter()
        {
            string path = "figuresXmlWriter.xml";
            XmlWriter writer;
            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Indent = true;
            writer = XmlWriter.Create(path, writerSettings);
            writer.WriteStartDocument();
            writer.WriteStartElement("figures");
            foreach (var figure in figures)
            {
                writer.WriteStartElement("figure");
                figure.WriteByXmlWriter(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public static void SaveFiguresByStreamWriter()
        {
            string path = "figuresStreamWriter.xml";
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine("<figures>");
            foreach (var figure in figures)
            {
                writer.WriteLine("<figure>");
                figure.WriteByStreamWriter(writer);
                writer.WriteLine("</figure>");
            }
            writer.WriteLine("</figures>");
            writer.Close();
        }

        public static void ReadFiguresByXmlReader()
        {
            string path = "figuresXmlWriter.xml";           
            XmlReader reader = XmlReader.Create(path);
            while (reader.Read())
            {
                if (reader.Name == "figure")
                {
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(reader.ReadOuterXml());
                    XmlNode node = document.SelectSingleNode("figure");
                    XmlNode figureTypeNode = node.SelectSingleNode("figuretype");
                    XmlNode colorNode = node.SelectSingleNode("color");
                    XmlNode materialNode = node.SelectSingleNode("material");

                    Colors color = (Colors)Enum.Parse(typeof(Colors), colorNode.InnerText);
                    Materials material = (Materials)Enum.Parse(typeof(Materials), materialNode.InnerText);

                    if (figureTypeNode.InnerText == "Square")
                    {
                        XmlNode squareSideNode = node.SelectSingleNode("a");
                        double squareSide = Convert.ToDouble(squareSideNode.InnerText);

                        Square square = new Square(material, squareSide);
                        if(material == Materials.Paper)
                        {
                            square.ToPaint(color);
                        }

                        figures.Add(square);
                    }
                    if (figureTypeNode.InnerText == "Circle")
                    {
                        XmlNode circleRadiusNode = node.SelectSingleNode("radius");
                        double circleRadius = Convert.ToDouble(circleRadiusNode.InnerText);

                        Circle circle = new Circle(material, circleRadius);
                        if (material == Materials.Paper)
                        {
                            circle.ToPaint(color);
                        }

                        figures.Add(circle);
                    }
                    if (figureTypeNode.InnerText == "Triangle")
                    {
                        XmlNode triangleSideNode = node.SelectSingleNode("a");
                        XmlNode triangleHeightNode = node.SelectSingleNode("height");
                        double triangleSide = Convert.ToDouble(triangleSideNode.InnerText);
                        double triangleHeight = Convert.ToDouble(triangleHeightNode.InnerText);

                        Triangle triangle = new Triangle(material, triangleSide, triangleHeight);
                        if (material == Materials.Paper)
                        {
                            triangle.ToPaint(color);
                        }

                        figures.Add(triangle);
                    }
                }  
            }
        }
    }
}