import {
  Button,
  Card,
  CardActions,
  CardContent,
  CardHeader,
  Paper,
  Typography,
} from "@mui/material";
import * as React from "react";

function Inicio() {
  return (
    <div className="container-fluid">
      <Paper elevation={3} style={{ padding: "16px", borderRadius: "8px" }}>
        <Card>
          <CardHeader
            className="text-center text-white bg-dark"
            title={<Typography variant="h4">SIADES - APP SU.</Typography>}
          />
          <CardContent style={{ display: "flex" }}>
            <div
              style={{
                width: "100%",
                height: "0",
                paddingBottom: "30%",
                margin: "2%",
                position: "relative",
                alignItems: "center",
              }}
            >
              <iframe
                src="https://giphy.com/embed/3aLoPy2B4t1GXJikgv"
                width="100%"
                height="100%"
                style={{ position: "absolute" }}
                class="giphy-embed"
                allowFullScreen
                title="s1"
              ></iframe>
            </div>
          </CardContent>
          <CardActions style={{ marginLeft: "45%" }}>
            <Button variant="contained" color="primary">
              Saiba Mais
            </Button>
          </CardActions>
        </Card>
      </Paper>
    </div>
  );
}
export default Inicio;
