﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM_LoginPage.Services
{
    public interface ISpeechToText
    {
        void StartSpeechToText();
        void StopSpeechToText();
    }
}
