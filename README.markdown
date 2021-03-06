ProxyBotLib
===========

A library for connecting a .NET based bot to the Starcraft Brood War API [http://code.google.com/p/bwapi/](http://code.google.com/p/bwapi/).
Contains source for the BWAPI socket communication dll and the .NET proxy bot.

Created for use with the EIS Starcraft AI competition [http://eis.ucsc.edu/StarCraftAICompetition](http://eis.ucsc.edu/StarCraftAICompetition)

Based on [http://breakablec.redirectme.net/svn/repos/trunk/StarProxyBot_net/](http://breakablec.redirectme.net/svn/repos/trunk/StarProxyBot_net/) 
which is based on [http://eis.ucsc.edu/StarCraftRemote](http://eis.ucsc.edu/StarCraftRemote)

Getting Started
---------------

This library is intended to aid an external, .NET based AI bot to connect to the BWAPI socket.
To get started, create a new console application which will be the base for your AI agent.
Add the ProxyBotLib project to the solution, and add to the console application a reference to ProxyBotLib.
There is a sample agent in ProxyBotLib.Tests that you should move to the console application.
Create an instance of the agent, and create an instance of the ProxyBot, passing the agent to the constructor of the ProxyBot.

Example
-------

    [STAThread]
    public static void Main(System.String[] args)
    {
        StarCraftAgent agent = new StarCraftAgent();
        ProxyBot proxyBot = new ProxyBot(agent);

        try
        {
            proxyBot.start();
        }
        catch (SocketException e)
        {
            Console.Error.WriteLine(e.StackTrace);
            System.Environment.Exit(0);
        }
        catch (System.Exception e)
        {
            Console.Error.WriteLine(e.StackTrace)
        }
    }