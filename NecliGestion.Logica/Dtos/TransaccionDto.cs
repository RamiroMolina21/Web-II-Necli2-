﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Dtos;

public record TransaccionDto(
  string CuentaDestino,
  decimal Monto,
  string Tipo
);
