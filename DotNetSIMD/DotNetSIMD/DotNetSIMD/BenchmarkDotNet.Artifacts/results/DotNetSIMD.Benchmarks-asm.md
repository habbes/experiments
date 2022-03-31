## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.MemberWiseSumScalar()
;     public void MemberWiseSumScalar() => Compute.MemberWiseSumScalar(A, B, C);
;                                          ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
       sub       rsp,28
       mov       rax,[rcx+8]
       mov       rdx,[rcx+10]
       mov       rcx,[rcx+18]
       mov       r8d,[rax+8]
       xor       r9d,r9d
       test      r8d,r8d
       jle       near ptr M00_L02
       test      rdx,rdx
       setne     r10b
       movzx     r10d,r10b
       test      rcx,rcx
       setne     r11b
       movzx     r11d,r11b
       test      r11d,r10d
       je        short M00_L01
       cmp       [rdx+8],r8d
       setge     r10b
       movzx     r10d,r10b
       mov       r11d,r8d
       not       r11d
       shr       r11d,1F
       and       r10d,r11d
       cmp       [rcx+8],r8d
       setge     r11b
       movzx     r11d,r11b
       test      r11d,r10d
       je        short M00_L01
M00_L00:
       movsxd    r10,r9d
       mov       r11d,[rax+r10*4+10]
       add       r11d,[rdx+r10*4+10]
       mov       [rcx+r10*4+10],r11d
       inc       r9d
       cmp       r9d,r8d
       jl        short M00_L00
       jmp       short M00_L02
M00_L01:
       movsxd    r11,r9d
       mov       r11d,[rax+r11*4+10]
       cmp       r9d,[rdx+8]
       jae       short M00_L03
       movsxd    r10,r9d
       add       r11d,[rdx+r10*4+10]
       cmp       r9d,[rcx+8]
       jae       short M00_L03
       movsxd    r10,r9d
       mov       [rcx+r10*4+10],r11d
       inc       r9d
       cmp       r9d,r8d
       jl        short M00_L01
M00_L02:
       add       rsp,28
       ret
M00_L03:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 184
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.MemberWiseSumSIMD()
;     public void MemberWiseSumSIMD() => Compute.MemberWiseSumSIMD(A, B, C);
;                                        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
       push      rsi
       sub       rsp,20
       vzeroupper
       mov       rax,[rcx+8]
       mov       rdx,[rcx+10]
       mov       rcx,[rcx+18]
       mov       r8d,[rax+8]
       mov       r9d,r8d
       xor       r10d,r10d
       test      r9d,r9d
       jle       short M00_L01
M00_L00:
       cmp       r10d,r8d
       jae       short M00_L02
       lea       r11d,[r10+7]
       cmp       r11d,r8d
       jae       short M00_L02
       vmovupd   ymm0,[rax+r10*4+10]
       mov       esi,[rdx+8]
       cmp       r10d,esi
       jae       short M00_L02
       cmp       r11d,esi
       jae       short M00_L02
       vmovupd   ymm1,[rdx+r10*4+10]
       vpaddd    ymm0,ymm0,ymm1
       mov       esi,[rcx+8]
       cmp       r10d,esi
       jae       short M00_L03
       cmp       r11d,esi
       jae       short M00_L04
       vmovupd   [rcx+r10*4+10],ymm0
       add       r10d,8
       cmp       r10d,r9d
       jl        short M00_L00
M00_L01:
       vzeroupper
       add       rsp,20
       pop       rsi
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
M00_L03:
       call      CORINFO_HELP_THROW_ARGUMENTOUTOFRANGEEXCEPTION
M00_L04:
       call      CORINFO_HELP_THROW_ARGUMENTEXCEPTION
       int       3
; Total bytes of code 134
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.ArraySumSalar()
;     public void ArraySumSalar() => Compute.ArraySumScalar(A);
;                                    ^^^^^^^^^^^^^^^^^^^^^^^^^
       mov       rax,[rcx+8]
       xor       edx,edx
       xor       ecx,ecx
       mov       r8d,[rax+8]
       test      r8d,r8d
       jle       short M00_L01
       nop       dword ptr [rax]
       nop       dword ptr [rax+rax]
M00_L00:
       movsxd    r9,ecx
       mov       r9d,[rax+r9*4+10]
       add       edx,r9d
       inc       ecx
       cmp       r8d,ecx
       jg        short M00_L00
M00_L01:
       ret
; Total bytes of code 51
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.ArraySumSIMD()
;     public void ArraySumSIMD() => Compute.ArraySumSIMD(A);
;                                   ^^^^^^^^^^^^^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       vxorps    ymm0,ymm0,ymm0
       xor       edx,edx
       mov       ecx,[rax+8]
       test      ecx,ecx
       jle       short M00_L01
       nop       dword ptr [rax]
M00_L00:
       cmp       edx,ecx
       jae       short M00_L02
       lea       r8d,[rdx+7]
       cmp       r8d,ecx
       jae       short M00_L02
       vmovupd   ymm1,[rax+rdx*4+10]
       vpaddd    ymm0,ymm0,ymm1
       add       edx,8
       cmp       ecx,edx
       jg        short M00_L00
M00_L01:
       vmovaps   xmm1,xmm0
       vmovd     eax,xmm1
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,1
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,2
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,3
       vextractf128 xmm1,ymm0,1
       vmovd     eax,xmm1
       vextractf128 xmm0,ymm0,1
       vpextrd   eax,xmm0,1
       vzeroupper
       add       rsp,28
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 136
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.MemberWiseSumScalar()
;     public void MemberWiseSumScalar() => Compute.MemberWiseSumScalar(A, B, C);
;                                          ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
       sub       rsp,28
       mov       rax,[rcx+8]
       mov       rdx,[rcx+10]
       mov       rcx,[rcx+18]
       mov       r8d,[rax+8]
       xor       r9d,r9d
       test      r8d,r8d
       jle       near ptr M00_L02
       test      rdx,rdx
       setne     r10b
       movzx     r10d,r10b
       test      rcx,rcx
       setne     r11b
       movzx     r11d,r11b
       test      r11d,r10d
       je        short M00_L01
       cmp       [rdx+8],r8d
       setge     r10b
       movzx     r10d,r10b
       mov       r11d,r8d
       not       r11d
       shr       r11d,1F
       and       r10d,r11d
       cmp       [rcx+8],r8d
       setge     r11b
       movzx     r11d,r11b
       test      r11d,r10d
       je        short M00_L01
M00_L00:
       movsxd    r10,r9d
       mov       r11d,[rax+r10*4+10]
       add       r11d,[rdx+r10*4+10]
       mov       [rcx+r10*4+10],r11d
       inc       r9d
       cmp       r9d,r8d
       jl        short M00_L00
       jmp       short M00_L02
M00_L01:
       movsxd    r11,r9d
       mov       r11d,[rax+r11*4+10]
       cmp       r9d,[rdx+8]
       jae       short M00_L03
       movsxd    r10,r9d
       add       r11d,[rdx+r10*4+10]
       cmp       r9d,[rcx+8]
       jae       short M00_L03
       movsxd    r10,r9d
       mov       [rcx+r10*4+10],r11d
       inc       r9d
       cmp       r9d,r8d
       jl        short M00_L01
M00_L02:
       add       rsp,28
       ret
M00_L03:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 184
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.MemberWiseSumSIMD()
;     public void MemberWiseSumSIMD() => Compute.MemberWiseSumSIMD(A, B, C);
;                                        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
       push      rsi
       sub       rsp,20
       vzeroupper
       mov       rax,[rcx+8]
       mov       rdx,[rcx+10]
       mov       rcx,[rcx+18]
       mov       r8d,[rax+8]
       mov       r9d,r8d
       xor       r10d,r10d
       test      r9d,r9d
       jle       short M00_L01
M00_L00:
       cmp       r10d,r8d
       jae       short M00_L02
       lea       r11d,[r10+7]
       cmp       r11d,r8d
       jae       short M00_L02
       vmovupd   ymm0,[rax+r10*4+10]
       mov       esi,[rdx+8]
       cmp       r10d,esi
       jae       short M00_L02
       cmp       r11d,esi
       jae       short M00_L02
       vmovupd   ymm1,[rdx+r10*4+10]
       vpaddd    ymm0,ymm0,ymm1
       mov       esi,[rcx+8]
       cmp       r10d,esi
       jae       short M00_L03
       cmp       r11d,esi
       jae       short M00_L04
       vmovupd   [rcx+r10*4+10],ymm0
       add       r10d,8
       cmp       r10d,r9d
       jl        short M00_L00
M00_L01:
       vzeroupper
       add       rsp,20
       pop       rsi
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
M00_L03:
       call      CORINFO_HELP_THROW_ARGUMENTOUTOFRANGEEXCEPTION
M00_L04:
       call      CORINFO_HELP_THROW_ARGUMENTEXCEPTION
       int       3
; Total bytes of code 134
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.ArraySumSalar()
;     public void ArraySumSalar() => Compute.ArraySumScalar(A);
;                                    ^^^^^^^^^^^^^^^^^^^^^^^^^
       mov       rax,[rcx+8]
       xor       edx,edx
       xor       ecx,ecx
       mov       r8d,[rax+8]
       test      r8d,r8d
       jle       short M00_L01
       nop       dword ptr [rax]
       nop       dword ptr [rax+rax]
M00_L00:
       movsxd    r9,ecx
       mov       r9d,[rax+r9*4+10]
       add       edx,r9d
       inc       ecx
       cmp       r8d,ecx
       jg        short M00_L00
M00_L01:
       ret
; Total bytes of code 51
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.ArraySumSIMD()
;     public void ArraySumSIMD() => Compute.ArraySumSIMD(A);
;                                   ^^^^^^^^^^^^^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       vxorps    ymm0,ymm0,ymm0
       xor       edx,edx
       mov       ecx,[rax+8]
       test      ecx,ecx
       jle       short M00_L01
       nop       dword ptr [rax]
M00_L00:
       cmp       edx,ecx
       jae       short M00_L02
       lea       r8d,[rdx+7]
       cmp       r8d,ecx
       jae       short M00_L02
       vmovupd   ymm1,[rax+rdx*4+10]
       vpaddd    ymm0,ymm0,ymm1
       add       edx,8
       cmp       ecx,edx
       jg        short M00_L00
M00_L01:
       vmovaps   xmm1,xmm0
       vmovd     eax,xmm1
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,1
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,2
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,3
       vextractf128 xmm1,ymm0,1
       vmovd     eax,xmm1
       vextractf128 xmm0,ymm0,1
       vpextrd   eax,xmm0,1
       vzeroupper
       add       rsp,28
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 136
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.MemberWiseSumScalar()
;     public void MemberWiseSumScalar() => Compute.MemberWiseSumScalar(A, B, C);
;                                          ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
       sub       rsp,28
       mov       rax,[rcx+8]
       mov       rdx,[rcx+10]
       mov       rcx,[rcx+18]
       mov       r8d,[rax+8]
       xor       r9d,r9d
       test      r8d,r8d
       jle       near ptr M00_L02
       test      rdx,rdx
       setne     r10b
       movzx     r10d,r10b
       test      rcx,rcx
       setne     r11b
       movzx     r11d,r11b
       test      r11d,r10d
       je        short M00_L01
       cmp       [rdx+8],r8d
       setge     r10b
       movzx     r10d,r10b
       mov       r11d,r8d
       not       r11d
       shr       r11d,1F
       and       r10d,r11d
       cmp       [rcx+8],r8d
       setge     r11b
       movzx     r11d,r11b
       test      r11d,r10d
       je        short M00_L01
M00_L00:
       movsxd    r10,r9d
       mov       r11d,[rax+r10*4+10]
       add       r11d,[rdx+r10*4+10]
       mov       [rcx+r10*4+10],r11d
       inc       r9d
       cmp       r9d,r8d
       jl        short M00_L00
       jmp       short M00_L02
M00_L01:
       movsxd    r11,r9d
       mov       r11d,[rax+r11*4+10]
       cmp       r9d,[rdx+8]
       jae       short M00_L03
       movsxd    r10,r9d
       add       r11d,[rdx+r10*4+10]
       cmp       r9d,[rcx+8]
       jae       short M00_L03
       movsxd    r10,r9d
       mov       [rcx+r10*4+10],r11d
       inc       r9d
       cmp       r9d,r8d
       jl        short M00_L01
M00_L02:
       add       rsp,28
       ret
M00_L03:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 184
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.MemberWiseSumSIMD()
;     public void MemberWiseSumSIMD() => Compute.MemberWiseSumSIMD(A, B, C);
;                                        ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
       push      rsi
       sub       rsp,20
       vzeroupper
       mov       rax,[rcx+8]
       mov       rdx,[rcx+10]
       mov       rcx,[rcx+18]
       mov       r8d,[rax+8]
       mov       r9d,r8d
       xor       r10d,r10d
       test      r9d,r9d
       jle       short M00_L01
M00_L00:
       cmp       r10d,r8d
       jae       short M00_L02
       lea       r11d,[r10+7]
       cmp       r11d,r8d
       jae       short M00_L02
       vmovupd   ymm0,[rax+r10*4+10]
       mov       esi,[rdx+8]
       cmp       r10d,esi
       jae       short M00_L02
       cmp       r11d,esi
       jae       short M00_L02
       vmovupd   ymm1,[rdx+r10*4+10]
       vpaddd    ymm0,ymm0,ymm1
       mov       esi,[rcx+8]
       cmp       r10d,esi
       jae       short M00_L03
       cmp       r11d,esi
       jae       short M00_L04
       vmovupd   [rcx+r10*4+10],ymm0
       add       r10d,8
       cmp       r10d,r9d
       jl        short M00_L00
M00_L01:
       vzeroupper
       add       rsp,20
       pop       rsi
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
M00_L03:
       call      CORINFO_HELP_THROW_ARGUMENTOUTOFRANGEEXCEPTION
M00_L04:
       call      CORINFO_HELP_THROW_ARGUMENTEXCEPTION
       int       3
; Total bytes of code 134
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.ArraySumSalar()
;     public void ArraySumSalar() => Compute.ArraySumScalar(A);
;                                    ^^^^^^^^^^^^^^^^^^^^^^^^^
       mov       rax,[rcx+8]
       xor       edx,edx
       xor       ecx,ecx
       mov       r8d,[rax+8]
       test      r8d,r8d
       jle       short M00_L01
       nop       dword ptr [rax]
       nop       dword ptr [rax+rax]
M00_L00:
       movsxd    r9,ecx
       mov       r9d,[rax+r9*4+10]
       add       edx,r9d
       inc       ecx
       cmp       r8d,ecx
       jg        short M00_L00
M00_L01:
       ret
; Total bytes of code 51
```

## .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```assembly
; DotNetSIMD.Benchmarks.ArraySumSIMD()
;     public void ArraySumSIMD() => Compute.ArraySumSIMD(A);
;                                   ^^^^^^^^^^^^^^^^^^^^^^^
       sub       rsp,28
       vzeroupper
       mov       rax,[rcx+8]
       vxorps    ymm0,ymm0,ymm0
       xor       edx,edx
       mov       ecx,[rax+8]
       test      ecx,ecx
       jle       short M00_L01
       nop       dword ptr [rax]
M00_L00:
       cmp       edx,ecx
       jae       short M00_L02
       lea       r8d,[rdx+7]
       cmp       r8d,ecx
       jae       short M00_L02
       vmovupd   ymm1,[rax+rdx*4+10]
       vpaddd    ymm0,ymm0,ymm1
       add       edx,8
       cmp       ecx,edx
       jg        short M00_L00
M00_L01:
       vmovaps   xmm1,xmm0
       vmovd     eax,xmm1
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,1
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,2
       vmovaps   xmm1,xmm0
       vpextrd   eax,xmm1,3
       vextractf128 xmm1,ymm0,1
       vmovd     eax,xmm1
       vextractf128 xmm0,ymm0,1
       vpextrd   eax,xmm0,1
       vzeroupper
       add       rsp,28
       ret
M00_L02:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 136
```

