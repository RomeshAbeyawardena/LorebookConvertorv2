﻿// See https://aka.ms/new-console-template for more information
using Lorebook.Convertor.Domain;
using System.Text.Json;

var lorebook = DeserialisedLorebook
    .ParseJson("C:\\Users\\romes\\Downloads\\New Hope (Mon Apr 01 2024).lorebook");
//"C:\\Users\\romes\\Downloads\\Battle of Orleans (Mon Apr 01 2024).lorebook");

File.WriteAllText("C:\\Users\\romes\\Downloads\\New Hope (Mon Apr 01 2024).json",
    //"Battle of Orleans (Sat Mar 30 2024).json",
       JsonSerializer.Serialize(lorebook));