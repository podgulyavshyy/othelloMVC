// See https://aka.ms/new-console-template for more information
using Othello.Model;
using Othello.Controller;
Console.WriteLine("Hello, World!");

var w = new Player("w");
var b = new Player("b");
var v = new Othello.View.View();
var i = new Game(w,b, v);
var c = new Controls();
c.StartGame(i);