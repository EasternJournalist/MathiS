using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScienceAndMath
{
    /// <summary>
    /// 表达式
    /// </summary>
    class Expression
    {
        /// <summary>
        /// 表达式文本内容
        /// </summary>
        string ExpressionText;

        public static bool IsAlphabet(char c)
        {
            return ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
        }

        public static bool IsOpenParen(char c)
        {
            return c == '{' || c == '[' || c == '(';
        }
        public static bool IsCloseParen(char c)
        {
            return c == '}' || c == ']' || c == ')';
        }
        public static bool IsArabic(char c)
        {
            return '0' <= c && c <= '9';
        }

        public static bool IsNameChar(char c)
        {
            return IsAlphabet(c) || IsArabic(c) || c == '@' || c == '#';
        }
        /// <summary>
        /// 从当前的左括号位置，找到与之匹配的右括号。其间只进行括号匹配检查，而不进行其他格式检查。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool GotoMatchedParen(ref string str ,int start, ref int i)
        {
            if (!IsOpenParen(str[start])) return false;
            char[] ParrenStack=new char[32];
            int Top = 0;
            i = start;
            ParrenStack[0] = str[i];
            i++;
            while (true)
            {
                if (Top == -1) break;
                if(i==str.Length)
                {
                    i = start;
                    return false;
                }
                if(IsOpenParen(str[i]))
                {//左括号入栈
                    ParrenStack[++Top] = str[i];
                    i++;
                    continue;
                }
                switch(str[i])//如果是右括号，判断是否匹配，匹配则使左括号出栈，不匹配返回false。如果不是括号什么都不做。
                {
                    case '}':
                        if (ParrenStack[Top] == '{') Top--; else { i = start;return false; }
                        break;
                    case ']':
                        if (ParrenStack[Top] == '[') Top--; else { i = start; return false; }
                        break;
                    case ')':
                        if (ParrenStack[Top] == '(') Top--; else { i = start; return false; }
                        break;
                }
                i++;
            }
            return true;
        }
        /// <summary>
        /// 识别上文环境,0：最低级环境（向量环境）；1：方程环境；2：多项式环境；3：项环境；4：因子环境；5：名称环境；6：不合法环境
        /// </summary>
        /// <param name="str"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int PreClass(ref string str, int i)
        {
            if (i == 0)
                return 0;
            else if(i==str.Length)
            {
                return 5;
            }
            else
            {
                i = i - 1;
                switch(str[i])
                {
                    case '[':
                    case '{':
                    case '(':
                        return 0;
                    case ',':
                        return 1;
                    case '=':
                    case '<':
                    case '>':
                        return 2;
                    case '+':
                    case '-':
                        return 3;
                    case '*':
                    case '/':
                        return 4;
                    case ')':
                    case ']':
                    case '}':
                        return 5;
                    default:
                        i++;
                        if (str[i] == '+' || str[i] == '-') return 3;
                        if (str[i] == '*' || str[i] == '/') return 4;
                        return 6;
                }
            }
        }
        /// <summary>
        /// 识别下文环境，识别上文环境,0：最低级环境（列表、向量环境）；1：方程环境；2：多项式环境；3：项环境；4：因子环境；5：名称环境；6：不合法环境
        /// </summary>
        /// <param name="str"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static int SucClass(ref string str, int i)
        {
            if (i == 0)
                return 5;
            if (i == str.Length)
                return 0;
            else
            {
                switch (str[i])
                {
                    case ']':
                    case '}':
                    case ')':
                        return 0;
                    case ',':
                        return 1;
                    case '=':
                    case '<':
                    case '>':
                        return 2;
                    case '+':
                    case '-':
                        return 3;
                    case '*':
                    case '/':
                        return 4;
                    case '(':
                    case '[':
                    case '{':
                        return 5;
                    default:
                        return 6;
                }
            }
        }
        
        /// <summary>
        /// 识别数字
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool RecFigure(ref string str, int start, ref int i)//识别实数字符串
        {
            if (start >= str.Length) return false;
            if (PreClass(ref str, start) > 4)//如果上文环境不合法，识别失败，返回false
            {
                return false;
            }
            else
            {
                for (; i < str.Length && IsArabic(str[i]) ; i++) ;
                if(SucClass(ref str,i)<=4)
                {
                    return true;
                }
                if(str[i]=='.')//数字带小数点，需要再识别小数位数
                {
                    i++;
                    for (; i < str.Length && IsArabic(str[i]); i++) ;
                    if (SucClass(ref str, i) <= 4)//下文环境级别小于等于4，识别成功
                        return true;
                    else//识别失败，需要将i位置重置
                    {
                        i = start;
                        return false;
                    }
                }
                //这里只针对识别实数的情况，增加识别复数的功能需要在这里加代码
                else
                {
                    i = start;
                    return false;
                }
            }
        }
        /// <summary>
        /// 识别名称
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool RecName(ref string str,int start,ref int i)
        {
            //if (start > str.Length) return false;在这里不需要因为PreClass会进行这个检查
            if (PreClass(ref str, start) > 5)//检查上文环境
            {
                return false;
            }
            else
            {
                i = start;
                if (IsAlphabet(str[i])||str[i]=='@'||str[i]=='#'||str[i]=='_'||str[i]=='~'||str[i]=='$')//判断首字符是否可以作为名称的首字符
                {
                    i++;
                    for (; i < str.Length && (IsAlphabet(str[i]) || IsArabic(str[i])); i++) ;
                    if(SucClass(ref str, i) <= 5)
                    {//是合法的名称
                        return true;
                    }
                    else
                    {
                        i = start;
                        return false;
                    }
                }
                else
                {
                    i = start;
                    return false;
                }
            }
            
        }
        /// <summary>
        /// 识别一个未定表达式的标识名称
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool RecNamep(ref string str, int start, ref int i)
        {
            i = start;

            if (str[i] == '@' || str[i] == '#' || str[i] == '_' || str[i] == '$')//判断是未定式的特殊符号开头
            {
                i++;
                for (; i < str.Length && (IsAlphabet(str[i]) || IsArabic(str[i])); i++) ;
                if (SucClass(ref str, i) <= 5 || str[i] == '@' || str[i] == '#'||str[i]=='$')
                {//是合法的名称。它的上下文环境可以是直接名称环境
                    return true;
                } 
                else
                {
                    i = start;
                    return false;
                }
            }
            else if(str[i]=='~')
            {
                if (PreClass(ref str, i) > 5) return false;//~要求完全合法的上下文环境，所以要进行上文检查
                for (; i < str.Length && (IsAlphabet(str[i]) || IsArabic(str[i])); i++) ;
                if (SucClass(ref str, i) <= 5)
                    return true;
                else
                {
                    i = start;
                    return false;
                }
            }
            else
            {
                i = start;
                return false;
            }
        }
        /// <summary>
        /// 识别因子
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool RecFactor(ref string str, int start ,ref int i)
        {
            if (start > str.Length) return false;
            if (str[start] == '/' || str[start] == '*')
            {//以乘除号开头的因子
                if (RecFactor(ref str, start + 1, ref i))//跳过当前位置的乘除号，从下一个位置开始识别
                    return true;
                else
                {
                    i = start;
                    return false;
                }
            }
            
            if (PreClass(ref str, start) > 4) return false;//检查上文环境
            i = start;

            if (IsNameChar(str[i]))
            {//名称字符开头的的因子，可能是数，变量名，或函数
                i = start;
                if (RecFigure(ref str, i, ref i)) return true;//是数字

                if(!RecName(ref str,i,ref i))//看是否能识别名称,识别名称失败说明不合法
                {
                    i = start;
                    return false;
                    
                }
                else
                {//是变量名或者函数

                    if (SucClass(ref str, i) <= 4) 
                            return true;//名称后不带参数,是变量
                    else
                    {//后有[]引导的参数或{}引导的列表，说明因子是函数或者向量、矩阵。支持若干个参数列表，如f[x,y][z][u,w][s],MAT{{a,b},{c,d}}
                        while (i < str.Length && (str[i] == '[' || str[i] == '{'))
                        {//这个循环实际上可以放在前面，但是这里用if语句分开，是为了分开变量和函数的情况，以便将来复制这里的代码
                            if (!GotoMatchedParen(ref str, i, ref i))
                            {
                                i = start;
                                return false;
                            }
                        }
                        if (SucClass(ref str, i) <= 4)//再看是否能正常结束
                            return true;
                        else
                        {
                            i = start;
                            return false;
                        }
                    }
                }
            }
            else if(IsOpenParen(str[i]))
            {//括号开头的因子,不管是{},[],()都正常读入，但是不允许多个连续括号，例如[1,3]是合法的，但[1,3][2,4]是非法的
                i = start;
                if (PreClass(ref str, i) > 4)//检查上文环境
                {
                    return false;
                }
                else
                {
                    if(!GotoMatchedParen(ref str,i,ref i))//检查括号匹配
                    {
                        i = start;
                        return false;
                    }
                    if (SucClass(ref str, i) <= 4)//检查下文环境
                        return true;
                    else
                    {
                        i = start;
                        return false;
                    }
                }
            }
            else
            {//其他情况都说明这不是因子
                i = start;
                return false;
            }
            
        }
        /// <summary>
        /// 识别项
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool RecItem(ref string str,int start,ref int i)
        {
            if (start > str.Length) return false;
            if (str[start]=='+'||str[start]=='-')
            {//正负号开头的项，跳过这个正负号，从下一个位置识别。把这个任务交给递归。
                if (RecItem(ref str, start + 1, ref i))
                    return true;
                else
                {
                    i = start;
                    return false;
                }
            }

            if (PreClass(ref str, start) > 4) return false;//检查上文环境，并不要求识别的项的上文必须是一个项的开始。如果前面是因子环境，也认为后面的所有因子组成的项是合法的。例如“a*b*c*d”,“b*c*d”就可以看做是一个项
            i = start;
            int tempi = i;
            if (!RecFactor(ref str, i, ref i))
                return false;
            while (SucClass(ref str, i) == 4)//识别所有连续的因子
            {
                if (!RecFactor(ref str, i, ref i))
                {
                    i = start;
                    return false;
                }
            }
                
            if (tempi == i)//不允许一个因子都没识别到
            {
                i = start;
                return false;
            }
            if (SucClass(ref str, i) <= 3)//下文环境级别必须低于项环境级别，识别成功
                return true;
            else
            {
                i = start;
                return false;
            }

        }

        /// <summary>
        /// 识别多项式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool RecPolynomial(ref string str, int start, ref int i)
        {
            //多项式没有特殊前缀，所以不需要类似s识别因子可以带*或/号，项可以带+或-号这些步骤。

            if (PreClass(ref str, start) > 3) return false;//检查上文环境。上文环境最高可以是项环境3,这使得a-b-c+d中，-b-c+d可以是一个项。
            i = start;
            int tempi = i;
            if (!RecItem(ref str, i, ref i)) return false;
            while (SucClass(ref str, i) == 3)//识别所有连续的项
            {
                if (!RecItem(ref str, i, ref i))
                {
                    i = start;
                    return false;
                }
            }
            if (tempi == i)//不允许一个项都没识别到
            {
                i = start;
                return false;
            }
            if (SucClass(ref str, i) <= 2)//下文环境也符合，识别成功
                return true;
            else
            {
                i = start;
                return false;
            }
        }
        /// <summary>
        /// 识别方程式。同时也可以识别多项式。当它识别到多项式时，IsFormula参数会是false，这时RecFormula的功能相当于RecPolynomial。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <param name="IsFormula"></param>
        /// <returns></returns>
        public static bool RecFormula(ref string str,int start,ref int i,ref bool IsFormula)//由于多项式和方程式属于不同级别，但为了区分，低级别并不兼容高级别：它要么是方程式，要么是多项式。列表中逗号之间，既可能是方程式也可能是多项式，但我们又不希望用RecFormula和RecPolynomial尝试识别两次，所以只识别一次， 加入IsFormula引用参数，如果识别到多项式，这一项是false。
        {
            i = start;
            if (PreClass(ref str, start) > 1) return false;//检查上文环境，必须是等式环境以下。这是因为方程不允许连等、连不等式表
            if (!RecPolynomial(ref str, i, ref i)) return false;//识别左边的多项式
            if (i == str.Length) { IsFormula = false;return true; }//已经到末尾，直接返回
            if (str[i] == '=' || str[i] == '<' || str[i] == '>')//如果有=<>，是方程
            {
                i++;
                if (!RecPolynomial(ref str, i, ref i)) { i = start;return false;}
                if (SucClass(ref str,i) > 1) { i = start;return false; }
                IsFormula = true;
                return true;
            }    
            else
            {//应该只是个多项式
                if(SucClass(ref str, i) <= 1)
                {
                    IsFormula = false;
                    return true;
                }
                else
                {
                    i = start;
                    return false;
                }
            }

        }
        /// <summary>
        /// 识别列表（向量）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public static bool RecList(ref string str, int start, ref int i)
        {
            i = start;
            bool isformula=false;
            while(true)
            {
                if (!RecFormula(ref str, i, ref i,ref isformula))
                {
                    i = start;
                    return false;
                }
                if (i == str.Length) return true; 
                if(str[i]==',')
                {
                    i++;
                }
                else
                {
                    if (IsCloseParen(str[i])) break;
                }
            }
            return true;
        }
        /// <summary>
        /// 返回表达式的级别.0：列表、向量；1：方程；2：多项式；3：项；4：因子；5：名称或数字；6：不合法
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ClassOf(ref string str)
        {
            int i=0;
            bool isformula = false ;
            RecFigure(ref str, 0, ref i);
            if (i == str.Length) return 5;
            RecName(ref str, 0, ref i);
            if (i == str.Length) return 5;
            RecFactor(ref str, 0, ref i);
            if (i == str.Length) return 4;
            if (!RecItem(ref str, 0, ref i)) return 6;
            if (i == str.Length) return 3;
            if(!RecFormula(ref str, 0, ref i,ref isformula))return 6;
            if (i == str.Length) return isformula ? 1 : 2;
            if (!RecList(ref str, 0, ref i)) return 6;
            if (i == str.Length) return 0; else return 6;
        }
        /// <summary>
        /// 判断在给定的上下文级别ContextClass中，是否能合法地分离出一个内文级别为InternalClass的表达式。、
        /// 通常内文级别要足够高，才使它能合法地作为一个整体分离
        /// </summary>
        /// <param name="ContextClass">上下文级别。指上文级别和下文级别中更高的那个级别</param>
        /// <param name="InternalClass">内文级别</param>
        /// <returns></returns>
        public static bool IsFeasibleClass(int contextClass,int internalClass)
        {
            switch(internalClass)
            {
                case 5:
                    if (contextClass <= 5) return true;
                    break;
                case 4:
                case 3:
                    if (contextClass <= 4) return true;
                    break;
                case 2:
                    if (contextClass <= 3) return true;
                    break;
                case 1:
                case 0:
                    if (contextClass <= 1) return true;
                    break;
            }
            return false;
        }
        public static bool IsFeasibleClass(int preClass,int sucClass,int internalClass)
        {
            int contextClass = preClass < sucClass ? sucClass : preClass;
            switch (internalClass)
            {
                case 5:
                    if (contextClass <= 5) return true;
                    break;
                case 4:
                case 3:
                    if (contextClass <= 4) return true;
                    break;
                case 2:
                    if (contextClass <= 3) return true;
                    break;
                case 1:
                case 0:
                    if (contextClass <= 1) return true;
                    break;
            }
            return false;
        }
        /// <summary>
        /// 对于给定上下文级别，中间的表达式级别最低多少合适(实际上就是上下文级别中较大的一个)
        /// </summary>
        /// <param name="preClass"></param>
        /// <param name="sucClass"></param>
        /// <returns></returns>
        public static int FeasibleClassFor(int preClass, int sucClass)
        {
            if (preClass == 6 || sucClass == 6) return -1;
            return sucClass >= preClass ? sucClass : preClass ;
        }
        /*public static int InnerClassOf(ref string str,int start,int end)
        {
            string s = str.Substring(start, end - start);
            int prec = PreClass(ref str, start);
            int succ = SucClass(ref str, end);
            return 0;
        }*/
        /// <summary>
        /// 从str的start位置，开始匹配patternStr。若匹配成功，返回true，i为str中匹配的部分后面一个位置，pairs为pattern的在str中匹配到的未定串。若匹配失败返回false。
        /// 为了节省时间它对pattternStr的语法合法性是完全信任的，如果patternStr不合法，必然会导致bug
        /// </summary>
        /// <param name="str"></param>
        /// <param name="istart"></param>
        /// <param name="patternStr">模式表达式字符串</param>
        /// <param name="patterClass">模式表达式级别</param>
        /// <returns></returns>
        public static bool Match(ref string str,int start,ref int i,ref string patternStr,int patternClass,ref Dictionary<string,string> pairs)
        {
            i = start;
            if(IsFeasibleClass(PreClass(ref str,start),patternClass))
            {
                string kstr, vstr;
                pairs.Clear();
                int j = 0,tempi,tempj;
                while(true)
                {
                    if (j == patternStr.Length )//已经匹配到末尾
                    {
                        if(IsFeasibleClass(SucClass(ref str, i), patternClass))//检查下文环境合法
                            return true;
                        else
                        {
                            i = start;
                            return false;
                        }
                    } 
                    if (i == str.Length) { i = start; return false; }
                    switch(patternStr[j])
                    {//名称、因子、项级别的，由于有时语法规则特殊，需要有特殊符号明确指出未定式级别。多项式级别的特殊符号是为了方便区分多项式与方程。~格式符是通用的。

                        case '_'://名称级别未定表达式
                            tempi = i;
                            tempj = j;
                            if(!RecNamep(ref patternStr, j, ref j)) { i = start; return false; }
                            if(!RecName(ref str, i, ref i)){ i = start; return false; }
                            break;
                        case '@'://因子级别未定表达式
                            tempi = i;
                            tempj = j;
                            if(!RecNamep(ref patternStr, j, ref j)) { i = start; return false; }
                            if (!RecFactor(ref str, i, ref i)) { i = start;return false; }
                            break;
                        case '#'://项级别未定表达式
                            tempi = i;
                            tempj = j;
                            if(!RecNamep(ref patternStr, j, ref j)) { i = start; return false; }
                            if (!RecItem(ref str, i, ref i)) { i = start; return false; } 
                            break;
                        case '$'://多项式级别未定表达式
                            tempi = i;
                            tempj = j;
                            if (!RecNamep(ref patternStr, j, ref j)) return false;
                            if (!RecPolynomial(ref str, i, ref i)) { i = start; return false; } 
                            break;
                        case '~'://根据上下文可行的最低级未定表达式
                            tempi = i;
                            tempj = j;
                            if(!RecNamep(ref patternStr, j, ref j)) { i = start;return false; }
                            switch(FeasibleClassFor(PreClass(ref patternStr,tempj),SucClass(ref patternStr,j)))
                            {
                                case 0:
                                    if (!RecList(ref str, i, ref i)) { i = start; return false; } 
                                    break;
                                case 1:
                                    bool isformula=false;
                                    if(!RecFormula(ref str, i, ref i,ref isformula)) { i = start; return false; }
                                    break;
                                case 2:
                                    if(!RecPolynomial(ref str, i, ref i)) { i = start; return false; }
                                    break;
                                case 3:
                                   if( !RecItem(ref str, i, ref i)) { i = start;return false; }
                                    break;
                                case 4:
                                    if (!RecFactor(ref str, i, ref i)) { i = start;return false; }
                                    break;
                                case 5:
                                    if(!RecName(ref str, i, ref i)) { i = start; return false; }
                                    break;
                                default:
                                    throw new Exception("模式串不合法错误");
                            }
                            break;
                        default://普通的逐一字符匹配
                            if (str[i] == patternStr[j])
                            {
                                i++; j++;
                            }
                            else
                            {
                                i = start;
                                return false;
                            }
                            continue;
                    }

                    kstr = patternStr.Substring(tempj, j - tempj);
                    vstr = str.Substring(tempi, i - tempi);
                    if(pairs.ContainsKey(kstr))//这个未定式是否以出现过
                    {
                        if(pairs[kstr]!=vstr)//如果出现过，那么它映射的表达式必须也相同，否则不匹配
                        {
                            i = start;
                            return false;
                        }
                    }
                    else
                    {//没出现过就直接加入
                        pairs.Add(kstr, vstr);
                    }
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 在str的第[start,end)个字符之间寻找和tarStr完全相同、环境合法的的子表达式串,返回找到的第一个匹配的起始位置。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="tarStr"></param>
        /// <param name="tarClass"></param>
        /// <returns></returns>
        public static int FindStr(ref string str, int start, int end, ref string tarStr,int tarClass)
        {
            end -= tarStr.Length;
            int j;
            for(int i = start; i < end; i++)
            {
                if (str[i] == tarStr[0]) continue;//先比较第0个字符。因为大多情况连第0个字符都不匹配，这样先把第一个就不匹配的情况先排除，可以节省很多时间。
                if (IsFeasibleClass(PreClass(ref str, i),SucClass(ref str,i+tarStr.Length), tarClass))
                {
                    for (j = 1; j < tarStr.Length; j++)//从第一个字符开始比较
                    {
                        if (str[i + j] != tarStr[j]) break;
                    }
                    if (j == tarStr.Length) return i;//如果匹配到最后一个位置之后，说明匹配成功，返回i
                }
            }
            return -1;//没有匹配到，返回-1
        }
        /// <summary>
        /// 将一个列表分成多个方程或多项式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="things"></param>
        /// <returns></returns>
        public static bool SplitList(ref string str,ref List<string> things)
        {
            things.Clear();
            int i = 0,tempi;
            bool isfm=false;
            while(i<str.Length)
            {
                tempi = i;
                if (!RecFormula(ref str, i, ref i, ref isfm)) return false;
                things.Add(str.Substring(tempi, i - tempi));
                i++;
            }
            return true;
        }
        /// <summary>
        /// 将一个多项式分为多个项
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Items"></param>
        /// <returns></returns>
        public static bool SplitPolynomial(ref string str, ref List<string> Items)
        {
            Items.Clear();
            int i = 0, tempi;

            while(i<str.Length)
            {
                tempi = i;
                if(!RecItem(ref str,i, ref i))return false;
                Items.Add(str.Substring(tempi, i - tempi));
            }
            return true;
        }
        /// <summary>
        /// 将一个项分为多个因子。这个项不能含有前导+或-。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Factors"></param>
        /// <returns></returns>
        public static bool SplitItem(ref string str,ref List<string> Factors)
        {
            Factors.Clear();
            int i = 0, tempi;
            
            while(i<str.Length)
            {
                tempi = i;
                if (!RecFactor(ref str, i, ref i)) return false;
                Factors.Add(str.Substring(tempi, i - tempi));
            }
            return true;
        }
        /// <summary>
        /// 将一个函数分为函数名和参数表达式
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="funcName"></param>
        /// <param name="paramexp"></param>
        /// <returns></returns>
        public static bool SplitFunctuionFactor(ref string exp,ref string funcName,ref List<string> paramexp)
        {
            int i = 0,tempi;
            if (!RecName(ref exp, 0, ref i)) return false;
            funcName = exp.Substring(0, i);
            while(i<exp.Length)
            {
                tempi = i+1;
                if (exp[i]!='['||!GotoMatchedParen(ref exp, i, ref i)) return false;
                if(i-1>tempi)
                {
                    paramexp.Add(exp.Substring(tempi, i - tempi - 1));
                }
            }
            return true;
        }
    }

    
}
