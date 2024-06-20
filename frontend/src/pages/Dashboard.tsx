import { Box, SimpleGrid } from "@chakra-ui/react";
import PerformanceByShapePlot from "../components/Plots/PerformanceByShapePlot";
import PerformanceByOperationPlot from "../components/Plots/PerformanceByOperationPlot";
import PerformanceByAreaPlot from "../components/Plots/PerformanceByAreaPlot";
import PerformanceByPerimiterPlot from "../components/Plots/PerformanceByPerimiterPlot";
import Top5Students from "../components/Plots/Top5Students";

export default function Dashboard() {
  return (
    <>
      <Box mb={5} height="500px">
        <Top5Students />
      </Box>
      <SimpleGrid columns={[1, 1, 2]} spacing="40px">
        <Box height="500px">
          <PerformanceByShapePlot />
        </Box>
        <Box height="500px">
          <PerformanceByOperationPlot />
        </Box>
        <Box height="500px">
          <PerformanceByAreaPlot />
        </Box>
        <Box height="500px">
          <PerformanceByPerimiterPlot />
        </Box>
      </SimpleGrid>
    </>
  );
}
