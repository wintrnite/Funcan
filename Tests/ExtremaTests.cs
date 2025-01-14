﻿using Funcan.Domain.Models;
using Funcan.Domain.Plotters;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class ExtremaTests {
    private readonly ExtremaPlotter extremaPlotter = new();

    [Test]
    public void TestSqr(){
        MathFunction.TryCreate("x^2", out var mathFunction);
        var range = new FunctionRange(-5, 5);
        var collection = extremaPlotter.GetPointSets(mathFunction, range);
        var pointSets = collection.ToList();
        Assert.AreEqual(1, pointSets.Count);
        var set = pointSets.First();
        Assert.AreEqual(1, set.Points.Count);
        Assert.Contains(new Point(0, 0), set.Points.ToList());
    }

    private void Polynomial(string expression, int count, params Point[] points){
        MathFunction.TryCreate(expression, out var mathFunction);
        var range = new FunctionRange(-5, 5);
        var collection = extremaPlotter.GetPointSets(mathFunction, range);
        var pointSets = collection.ToList();
        Assert.AreEqual(1, pointSets.Count);
        var set = pointSets.First();
        Assert.AreEqual(count, set.Points.Count);
        var tuples = points.Zip(set.Points);
        var delta = 0.1;
        foreach (var tuple in tuples){
            Assert.AreEqual(tuple.First.X, tuple.Second.X, delta);
            Assert.AreEqual(tuple.First.Y, tuple.Second.Y, delta);
        }
    }

    [Test]
    public void TestPolynomial(){
        Polynomial("x^2 - 5x + 4", 1, new Point(2.5, -2.25));
        Polynomial("x^3 + x^2", 2, new Point(-0.667, 0.148), new Point(0, 0));
    }

    private void Trigonometry(string function, params Point[] points){
        MathFunction.TryCreate(function, out var mathFunction);
        var range = new FunctionRange(-5, 7);
        var collection = extremaPlotter.GetPointSets(mathFunction, range);
        var pointSets = collection.ToList();
        Assert.True(pointSets.Count != 0);
        var set = pointSets.First();

        foreach (var point in set.Points)
            Assert.Contains(point, points);
    }

    [Test]
    public void TestTrigonometry(){
        var pi = Math.PI;
        Trigonometry("sin(x)", new Point(pi * -1.5, 1), new Point(pi * -0.5, -1),
            new Point(pi * 0.5, 1), new Point(pi * 1.5, -1));
        Trigonometry("cos(x)", new Point(pi * -1, -1), new Point(0, 1),
            new Point(pi, -1), new Point(pi * 2, 1));
    }
}