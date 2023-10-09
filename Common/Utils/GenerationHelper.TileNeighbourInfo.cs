using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace BossRush.Common.Utils
{
    internal partial class GenerationHelper
    {
        public record TileNeighbourInfo(int I, int J)
        {
            private static bool CoordinatesOutOfBounds(int i, int j) => i >= Main.maxTilesX || j >= Main.maxTilesY || i < 0 || j < 0;
            public abstract record CountableNeighbourInfo(int I, int J)
            {
                protected abstract bool ShouldCount(Tile tile);

                private bool? top;
                public bool Top => top ??= (CoordinatesOutOfBounds(I, J - 1) || ShouldCount(Main.tile[I, J - 1]));

                private bool? topRight;
                public bool TopRight => topRight ??= (CoordinatesOutOfBounds(I + 1, J - 1) || ShouldCount(Main.tile[I + 1, J - 1]));

                private bool? topleft;
                public bool TopLeft => topleft ??= (CoordinatesOutOfBounds(I - 1, J - 1) || ShouldCount(Main.tile[I - 1, J - 1]));

                private bool? bottom;
                public bool Bottom => bottom ??= (CoordinatesOutOfBounds(I, J + 1) || ShouldCount(Main.tile[I, J + 1]));

                private bool? bottomRight;
                public bool BottomRight => bottomRight ??= (CoordinatesOutOfBounds(I + 1, J + 1) || ShouldCount(Main.tile[I + 1, J + 1]));

                private bool? bottomLeft;
                public bool BottomLeft => bottomLeft ??= (CoordinatesOutOfBounds(I - 1, J + 1) || ShouldCount(Main.tile[I - 1, J + 1]));

                private bool? right;
                public bool Right => right ??= (CoordinatesOutOfBounds(I + 1, J) || ShouldCount(Main.tile[I + 1, J]));

                private bool? left;
                public bool Left => left ??= (CoordinatesOutOfBounds(I - 1, J) || ShouldCount(Main.tile[I - 1, J]));

                private int? count;
                public int Count => count ??= (Top ? 1 : 0)
                                + (TopRight ? 1 : 0)
                                + (TopLeft ? 1 : 0)
                                + (Bottom ? 1 : 0)
                                + (BottomRight ? 1 : 0)
                                + (BottomLeft ? 1 : 0)
                                + (Right ? 1 : 0)
                                + (Left ? 1 : 0);
            }

            public record PredicateNeighbourInfo(int I, int J, Func<Tile, bool> Predicate) : CountableNeighbourInfo(I, J)
            {
                protected override bool ShouldCount(Tile tile) => Predicate.Invoke(tile);
            }

            public record TypeCountInfo(int I, int J, Func<Tile, bool> Predicate) : CountableNeighbourInfo(I, J)
            {
                public ushort MaxCountType => TileCounts.Aggregate((pair1, pair2) => pair1.Value > pair2.Value ? pair1 : pair2).Key;
                public ushort MinCountType => TileCounts.Aggregate((pair1, pair2) => pair1.Value < pair2.Value ? pair1 : pair2).Key;

                public Dictionary<ushort, int> TileCounts
                {
                    get
                    {
                        if (tileCounts is null)
                        {
                            tileCounts = new();
                            for (int i = I; i < I + 3; i++)
                            {
                                for (int j = J; j < J + 3; j++)
                                {
                                    if ((i == I + 1 && j == J + 1) || CoordinatesOutOfBounds(i, j))
                                    {
                                        continue;
                                    }

                                    Tile tile = Main.tile[i, j];

                                    if (!tileCounts.ContainsKey(tile.TileType))
                                    {
                                        tileCounts[tile.TileType] = 0;
                                    }

                                    if (Predicate is null || Predicate(tile))
                                    {
                                        tileCounts[tile.TileType]++;
                                    }
                                }
                            }
                        }

                        return tileCounts;
                    }
                }

                private Dictionary<ushort, int> tileCounts;

                protected override bool ShouldCount(Tile tile)
                {
                    return Predicate is null || Predicate(tile);
                }
            }

            private PredicateNeighbourInfo hasTile;
            public PredicateNeighbourInfo HasTile => hasTile ??= new(I, J, tile => tile.HasTile);

            private PredicateNeighbourInfo solid;
            public PredicateNeighbourInfo Solid => solid ??= new(I, J, tile => tile.HasTile && tile.BlockType == BlockType.Solid);

            private PredicateNeighbourInfo sloped;
            public PredicateNeighbourInfo Sloped => sloped ??= new(I, J, tile => tile.HasTile && tile.BlockType != BlockType.Solid);

            private PredicateNeighbourInfo wall;
            public PredicateNeighbourInfo Wall => wall ??= new(I, J, tile => tile.WallType != WallID.None);

            public TypeCountInfo TypeCount(Func<Tile, bool> predicate = null) => new(I, J, predicate);

            public PredicateNeighbourInfo TypedSolid(ushort type) => new(I, J, tile => tile.HasTile && tile.TileType == type);

            public PredicateNeighbourInfo GetPredicateNeighbourInfo(Func<Tile, bool> predicate) => new(I, J, predicate);
        }
    }
}
