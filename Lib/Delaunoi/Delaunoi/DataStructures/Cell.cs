﻿using System;
using System.Linq;
using System.Collections.Generic;


namespace Delaunoi.DataStructures
{
    /// <summary>
    /// Abstraction of a triangulation (primal graph) dual graph element.
    /// </summary>
    public class Cell<T>
    {
        private static int nextID = 0;
        private int _id;

        private QuadEdge<T> _primal;
        private bool        _onBounds;
        private bool        _reconstructed;

        /// <summary>
        /// Construct a cell abstraction based on a <paramref name="primal"/> edge.
        /// </summary>
        /// <param name="primal">Delaunay edge with Origin as cell center.</param>
        /// <param name="onBounds">Mark cell as a cell on the cells bounds.</param>
        /// <param name="reconstructed">Must be true if cell has a site at infinity.</param>
        public Cell(QuadEdge<T> primal, bool onBounds, bool reconstructed)
        {
            this._primal  = primal;
            this._onBounds = onBounds;
            this._reconstructed = reconstructed;

            this._id = nextID++;
        }

        /// <summary>
        /// Get primal Edge with Origin as cell center.
        /// </summary>
        public QuadEdge<T> PrimalEdge
        {
            get {return _primal;}
        }

        /// <summary>
        /// True if cell primal edge origin is on the convex hull of Delaunay triangulation.
        /// </summary>
        public bool IsOnBounds
        {
            get {return _onBounds;}
        }

        /// <summary>
        /// True if cell has been constructed because of a missing primal site (at infinity).
        /// </summary>
        public bool Reconstructed
        {
            get {return _reconstructed;}
        }

        /// <summary>
        /// Get cell center
        /// </summary>
        public Vec3 Center
        {
            get {return _primal.Origin;}
        }

        public int ID
        {
            get {return _id;}
        }

        /// <summary>
        /// Get cell boundary points.
        /// </summary>
        public IEnumerable<Vec3> Bounds
        {
            get
            {
                // Handle missing edge at infinity
                if (_onBounds)
                {
                    yield return _primal.Rot.Destination;
                }

                // Bounds
                foreach (Vec3 site in _primal.CellLeftVertices())
                {
                    yield return site;
                }
            }
        }

        /// <summary>
        /// Get all points. Center is first one followed by cell bounds.
        /// </summary>
        public IEnumerable<Vec3> Points
        {
            get
            {
                // Center
                yield return _primal.Origin;
                // Bounds
                foreach (Vec3 site in Bounds)
                {
                    yield return site;
                }
            }
        }
    }
}
