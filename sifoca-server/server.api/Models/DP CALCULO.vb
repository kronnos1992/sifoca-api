'##################### SOMENTE PARA DANOS PRÓPRIOS #######################
#Region "Cáculo de Danos Próprios"

If Me.CMB_PLANO.Text = "DANOS PRÓPRIOS" Then
    Try
        If Val(LBL_CAP_AQUISICAO.Text) < 1 Then
            MessageBoxEx.Show(LBL_MATRICULA.Text + vbCrLf + "Falta valor do capital de aquisição", "APÓLICE", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            DESV.DESVALORIZACAO_AUTO((Now.Year - CInt(LBL_IDADE.Text)))
            Dim premio_simples_dp As Double = 0
            'Dim cr As Double = (((-LBL_CAP_AQUISICAO.Text) * (LBL_PERC_IDADE.Text / 100)) + LBL_CAP_AQUISICAO.Text)
            'Dim pdp As Double = (cr * (LBL_PERC_CATEGORIA.Text / 100))
            'premio_simples = premio_simples + pdp
            'premio_simples_ = premio_simples_ + pdp
            'MsgBox("Ano de Desv:" & (Now.Year - CInt(LBL_IDADE.Text)) & vbCrLf & "Cr:" & cr & vbCrLf & "pdp: " & pdp & vbCrLf & "premio: " & premio_simples)
            VALOR_DESVALORIZADO = 0
            VALOR_DESVALORIZADO = Val(LBL_CAP_AQUISICAO.Text) - Val((LBL_CAP_AQUISICAO.Text * LBL_PERC_IDADE.Text) / 100)
            'Achar o somatório das coberturas selecionadas de percentagem
            For Each d1 As DataGridViewRow In DGV_COBERTURA.Rows
                If (CBool(d1.Cells("SEL").Value) = True And CBool(d1.Cells("PER?").Value) = True) Then
                    If (CStr(d1.Cells(1).Value) <> "Quebra Isolada de Vidro") Then
                        premio_simples_dp = premio_simples_dp + ((VALOR_DESVALORIZADO * d1.Cells("PERC").Value) / 100)
                    End If
                End If
            Next
            'alterar o valor da  cobertura para o valor desvalorizado.
            DGV_COBERTURA.Rows(1).Cells("COBERTURA").Value = VALOR_DESVALORIZADO
            DGV_COBERTURA.Rows(2).Cells("COBERTURA").Value = VALOR_DESVALORIZADO
            DGV_COBERTURA.Rows(3).Cells("COBERTURA").Value = VALOR_DESVALORIZADO
            'Achar o premio de QUEBRA ISOLADA, caso for por percentagem
            For Each d4 As DataGridViewRow In DGV_COBERTURA.Rows
                If (CBool(d4.Cells("SEL").Value) = True And CBool(d4.Cells("PER?").Value) = True And (CStr(d4.Cells(1).Value) = "Quebra Isolada de Vidro")) Then
                    premio_simples_dp = premio_simples_dp + ((CDbl(d4.Cells("COBERTURA").Value) * d4.Cells("PERC").Value) / 100)
                End If
            Next
            'Achar o somatório das coberturas selecionadas de valor absoluto com excepção percentagem
            For Each d2 As DataGridViewRow In DGV_COBERTURA.Rows
                If (CBool(d2.Cells("SEL").Value) = True And CBool(d2.Cells("PER?").Value) = False And ((CStr(d2.Cells(1).Value) <> "Responsabilidade Civil Obrigatoria"))) Then ' Or (CStr(d2.Cells(1).Value) <> "Responsabilidade Civil Obrigatória"))) Then
                    If (CStr(d2.Cells(1).Value) <> "Ocupantes") Then
                        premio_simples_dp = premio_simples_dp + d2.Cells("PERC").Value
                    End If
                End If
            Next
            'SOMAR TODOS PREMIOS PELO CAPITAL DE OCUPANTES
            For Each d3 As DataGridViewRow In DGV_COBERTURA.Rows
                If (CBool(d3.Cells("SEL").Value) = True And CBool(d3.Cells("PER?").Value) = False And CStr(d3.Cells(1).Value) = "Ocupantes") Then
                    premio_simples_dp = premio_simples_dp + LBL_PREMIO_OCUPANTE.Text
                End If
            Next
            premio_simples = premio_simples_dp + valor
            premio_simples_ = premio_simples_dp + valor
        End If
    Catch ex As Exception
        MessageBoxEx.Show("Falta dados para cálculos de danos próprios", "APÓLICE", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Try
End If