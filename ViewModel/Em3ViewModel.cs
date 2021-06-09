﻿using System;
using System.Collections.ObjectModel;
using EM3.Annotations;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EM3.Model;

namespace EM3.ViewModel
{
    public class Em3ViewModel : INotifyPropertyChanged
    {
        #region Props

        private static string _out = string.Empty;
        private static string _errors = string.Empty;
        private static string _fileContent;
        private static string _commands;
        private static string _outReg;
        private static string _regA;
        private static string _regB;
        private static string _other;
        private bool _isBreak = false;
        private int _jmpCount = 0;

        public string FileContent
        {
            get => _fileContent;
            set
            {
                _fileContent = value;
                OnPropertyChanged();
            }
        }
        public string Commands
        {
            get => _commands;
            set
            {
                _commands = value;
                OnPropertyChanged();
            }
        }
        public string OutReg
        {
            get => _outReg;
            set
            {
                _outReg = value;
                OnPropertyChanged();
            }
        }
        public string RegA
        {
            get => _regA;
            set
            {
                _regA = value;
                OnPropertyChanged();
            }
        }
        public string RegB
        {
            get => _regB;
            set
            {
                _regB = value;
                OnPropertyChanged();
            }
        }
        public string Other
        {
            get => _other;
            set
            {
                _other = value;
                OnPropertyChanged();
            }
        }
        public string Out
        {
            get => _out;
            set
            {
                _out = value;
                OnPropertyChanged();
            }
        }

        public string Errors
        {
            get => _errors;
            set
            {
                _errors = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Register> Registers { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private static Parser parser = new();
        private Compiler _compiler;

        #endregion


        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Em3ViewModel()
        {
            Registers = new();
            _compiler = new();
        }

        

        private void Run()
        {
            var com = parser.SplitAndFormat(Commands);
            var outReg = parser.SplitAndFormat(OutReg);
            var regA = parser.SplitAndFormat(RegA.Trim());
            var regB = parser.SplitAndFormat(RegB.Trim());

            bool result = _compiler.Compile(com, outReg, regA, regB);

            if (!result)
            {
                Errors = string.Join('\n', _compiler.ErrorProvider.GetErrors());

                return;
            }

            for (int i = 0; i < com.Length && !_isBreak; i++)
            {

                if (com[i] == OpertationEnum.Sum.ToString())
                {

                    double var1 = 0;
                    if (!double.TryParse(regA[i], out var1))
                    {
                        int _reg = int.Parse(regA[i].Substring(0, regA[i].Length - 1));
                        var1 = (double) Registers.FirstOrDefault((r) => r.Num == _reg).Value;
                    }

                    // sum1 = double.Parse(data[i][2]);
                    double var2 = 0; //double.Parse(data[i][3]);
                    if (!double.TryParse(regB[i], out var2))
                    {
                        int _reg = int.Parse(regB[i].Substring(0, regB[i].Length - 1));
                        var2 = (double) Registers.FirstOrDefault((r) => r.Num == _reg).Value;
                    }

                    double res = Operations.Sum(var1, var2);

                    int register = int.Parse(outReg[i]);

                    Register reg = Registers.FirstOrDefault((r) => r.Num == register);

                    if (reg != null)
                        reg.Value = res;
                    else
                        Registers.Add(new(res, register));
                }
                else if (com[i] == OpertationEnum.Subtr.ToString())
                {
                    double var1 = 0;
                    if (!double.TryParse(regA[i], out var1))
                    {
                        int _reg = int.Parse(regA[i].Substring(0, regA[i].Length - 1));
                        var1 = (double) Registers.FirstOrDefault((r) => r.Num == _reg).Value;
                    }

                    // sum1 = double.Parse(data[i][2]);
                    double var2 = 0; //double.Parse(data[i][3]);
                    if (!double.TryParse(regB[i], out var2))
                    {
                        int _reg = int.Parse(regB[i].Substring(0, regB[i].Length - 1));
                        var2 = (double) Registers.FirstOrDefault((r) => r.Num == _reg).Value;
                    }

                    double res = Operations.Subtr(var1, var2);

                    int register = int.Parse(outReg[i]);

                    Register reg = Registers.FirstOrDefault((r) => r.Num == register);

                    if (reg != null)
                        reg.Value = res;
                    else
                        Registers.Add(new(res, register));
                }
                else if (com[i] == OpertationEnum.Div.ToString())
                {
                    double var1 = 0;
                    if (!double.TryParse(regA[i], out var1))
                    {
                        int _reg = int.Parse(regA[i].Substring(0, regA[i].Length - 1));
                        var1 = (double) Registers.FirstOrDefault((r) => r.Num == _reg).Value;
                    }

                    // sum1 = double.Parse(data[i][2]);
                    double var2 = 0; //double.Parse(data[i][3]);
                    if (!double.TryParse(regB[i], out var2))
                    {
                        int _reg = int.Parse(regB[i].Substring(0, regB[i].Length - 1));
                        var2 = (double) Registers.FirstOrDefault((r) => r.Num == _reg).Value;
                    }

                    double res = Operations.Div(var1, var2);

                    int register = int.Parse(outReg[i]);

                    Register reg = Registers.FirstOrDefault((r) => r.Num == register);

                    if (reg != null)
                        reg.Value = res;
                    else
                        Registers.Add(new(res, register));
                }
                else if (com[i] == OpertationEnum.Mult.ToString())
                {
                    double var1 = 0;
                    if (!double.TryParse(regA[i], out var1))
                    {
                        int _reg = int.Parse(regA[i].Substring(0, regA[i].Length - 1));
                        var1 = (double) Registers.FirstOrDefault((r) => r.Num == _reg).Value;
                    }

                    // sum1 = double.Parse(data[i][2]);
                    double var2 = 0; //double.Parse(data[i][3]);
                    if (!double.TryParse(regB[i], out var2))
                    {
                        int _reg = int.Parse(regB[i].Substring(0, regB[i].Length - 1));
                        var2 = (double) Registers.FirstOrDefault((r) => r.Num == _reg).Value;
                    }

                    double res = Operations.Mult(var1, var2);

                    int register = int.Parse(outReg[i]);

                    Register reg = Registers.FirstOrDefault((r) => r.Num == register);

                    if (reg != null)
                        reg.Value = res;
                    else
                        Registers.Add(new(res, register));
                }
                else if (com[i] == OpertationEnum.CrtVarD.ToString())
                {
                    int register = int.Parse(outReg[i]);
                    double value = double.Parse(regA[i]);


                    Registers.Add(Operations.CrtVarD(value, register));
                }
                else if (com[i] == OpertationEnum.Jmp.ToString())
                {
                    int line = int.Parse(outReg[i]);
                    int to = int.Parse(regA[i]);

                    if (_jmpCount < to - 1)
                    {
                        i = line - 2;
                        _jmpCount++;
                    }
                    else
                    {
                        _jmpCount = 0;
                    }
                }
                else if (com[i] == OpertationEnum.Out.ToString())
                {
                    int numRegister = int.Parse(outReg[i]);

                    var value = Registers.FirstOrDefault((r => numRegister == r.Num)).Value;
                    if (value != null)
                        Out = Operations.Out(value.Value);
                }
                else if (com[i] == OpertationEnum.End.ToString())
                {
                    _isBreak = true;
                }

                if (_isBreak) break;
            }
        }

        private void Reset()
        {
            Registers.Clear();
            _jmpCount = 0;
            _isBreak = false;
            Errors = string.Empty;
            OnPropertyChanged("Errors");
        }

        public Em3Command RunCommand => new(o =>
        {
            Reset();
            
            Run();
        });

        public Em3Command ParseCommand => new(o =>
        {
            var data = parser.ParseData(FileContent);
            Commands = data[0];
            OutReg = data[1];
            RegA = data[2];
            RegB = data[3];
            Other = data[4];
        });
    }
}
