﻿using System.Collections.Concurrent;

namespace RemotelyControlledRobot.Framework.System.Bus;

internal class CommandSubscribersRepository
{
    private readonly ConcurrentDictionary<string, List<Action<object?>>> _commandSubscribers = new();

    public IReadOnlyList<Action<object?>> Get(string command)
    {
        return _commandSubscribers.GetValueOrDefault(command) ?? new List<Action<object?>>();
    }

    public void Add(string command, Action<object?> messageCallback)
    {
        if (!_commandSubscribers.ContainsKey(command))
        {
            _commandSubscribers[command] = new List<Action<object?>>();
        }

        if (!_commandSubscribers[command].Contains(messageCallback))
        {
            _commandSubscribers[command].Add(messageCallback);
        }
    }

    public void Remove(string command, Action<object?> messageCallback)
    {
        if (_commandSubscribers.ContainsKey(command))
        {
            if (_commandSubscribers[command].Contains(messageCallback))
            {
                _commandSubscribers[command].Remove(messageCallback);
            }

            if (!_commandSubscribers[command].Any())
            {
                _commandSubscribers.Remove(command, out _);
            }
        }
    }
}