using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain.Enums
{
    public enum HardwareType
    {
        ModemModel = 1010,
        ModemSrNo = 1011,
        MACAirNo = 1012,
        AntennaSize = 1013,
        TransceiverWATT = 1014,
        TransceiverSrNo = 1015,
        HardwareCondition=1018,


        #region Hardware Types
        Modem = 79,
        AntennaMeter = 81,
        Kit = 80,
        Spare = 83,
        Transceiver = 82
        #endregion
    }
}
