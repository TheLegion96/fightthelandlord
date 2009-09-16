﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace SilverlightAStar
{
    public struct PathNote
    {
        public int F; //F = G + H
        public int G;
        public int H;
        public int X;
        public int Y;
        public PathNote parentNote;
    }

    public class PathFinder
    {
        private byte[,] matrix;
        private Point startPoint;
        private Point endPoint;
        private List<PathNote> openedList = new List<PathNote>();
        private List<PathNote> colsedList = new List<PathNote>();
        private sbyte[,] direction = new sbyte[8,2]{ {0,-1} , {1,0}, {0,1}, {-1,0}, {1,-1}, {1,1}, {-1,1}, {-1,-1}};
        public PathFinder(byte[,] matrix, Point startPoint, Point endPoint)
        {
            this.matrix = matrix;
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        private void GetPathNote(PathNote parentNote)
        {
            var pathNote = new PathNote() { parentNote = parentNote };
        }

        public List<Point> StartFindPath(PathNote pathNote)
        {
            for (int i = 0; i < 8; i++)
            {
                var newPathNote = new PathNote();
                newPathNote.parentNote = pathNote;
                newPathNote.X = newPathNote.parentNote.X + direction[i, 0];
                newPathNote.Y = newPathNote.parentNote.Y + direction[i, 1];

                if (newPathNote.X < 0 || newPathNote.Y < 0 || newPathNote.X > matrix.GetUpperBound(0) || newPathNote.Y > matrix.GetUpperBound(1)) //坐标过节
                    continue;

                bool isClosed = false;
                bool isOpened = false;
                PathNote theOpendandSameNote = new PathNote();
                foreach (var closedNote in colsedList)
                {
                    if (closedNote.X == newPathNote.X && closedNote.Y == newPathNote.Y)
                    {
                        isClosed = true;
                    }
                }

                foreach (var openedNote in openedList)
                {
                    if (openedNote.X == newPathNote.X && openedNote.Y == newPathNote.Y)
                    {
                        isOpened = true;
                        theOpendandSameNote = openedNote;
                    }
                }

                if (matrix[newPathNote.X, newPathNote.Y] != 0 || isClosed) //不能通行或者已经在关闭列表中存在
                    continue;

                if (Math.Abs(direction[i, 0] + direction[i, 1]) == 2)  //G值
                {
                    newPathNote.G = newPathNote.parentNote.G + 14;
                }
                else
                {
                    newPathNote.G = newPathNote.parentNote.G + 10;
                }

                newPathNote.H = (int)(Math.Abs(endPoint.X - newPathNote.X) + Math.Abs(endPoint.Y - newPathNote.Y)) * 10;  //H值
                newPathNote.F = newPathNote.G + newPathNote.H;  //F值


                if (isOpened)
                {
                    if (newPathNote.G >= theOpendandSameNote.G)
                        continue;
                    else
                    {
                        this.openedList.Remove(theOpendandSameNote);
                        this.openedList.Add(newPathNote);
                        continue;
                    }
                }

                this.openedList.Add(newPathNote);
                this.openedList.Remove(pathNote);
                this.colsedList.Add(pathNote);
                this.openedList.Sort(Compare);

                foreach (var openedNote in openedList)
                {
                    if (openedNote.X == endPoint.X && openedNote.Y == endPoint.Y)
                    {

                    }
                }

                StartFindPath(openedList[0]);
            }
        }

        private List<PathNote> GetPathNoteListByParent(PathNote pathNote, List<PathNote> pathNotes)
        {
            if (pathNotes == null)
                pathNotes = new List<PathNote>();
            if (pathNote.Equals(pathNote.parentNote))
                return pathNotes;
            pathNotes.Add(pathNote.parentNote);
            GetPathNoteListByParent(pathNote, pathNotes);
        }

        private int Compare(PathNote x, PathNote y)
        {
            if (x.F > y.F)
                return 1;
            else if (x.F < y.F)
                return -1;
            return 0;
        }
    }
}
