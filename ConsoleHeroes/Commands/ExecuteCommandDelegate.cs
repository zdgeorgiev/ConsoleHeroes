﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHeroes.Commands
{
    public delegate void ExecuteCommandDelegate(object obj);
    public delegate bool CanExecuteCommandDelegate(object obj);
}
