using System.IO;
using System.Linq;

namespace VoxFirmwareRetract
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 0 && System.IO.File.Exists(args[0]))
            {
                int retractionCount = 0;
                var dir = System.IO.Path.GetDirectoryName(args[0]);
                var file = System.IO.Path.GetFileNameWithoutExtension(args[0]);

                var newPath = Path.Combine(dir, file);

                using (var gcodeFile = new System.IO.StreamReader(args[0]))
                {

                    using (var newFile = new System.IO.StreamWriter(newPath + ".FirmwareRetract.gcode"))
                    {

                        while (!gcodeFile.EndOfStream)
                        {
                            var line = gcodeFile.ReadLine();
                            var tokens = line.Split(' ');

                            if (tokens.Any(t => t.Contains("F60000")))
                            {
                                var extrusion = tokens.FirstOrDefault(t => t.StartsWith("E"));
                                if (extrusion == null)
                                {
                                    continue;
                                }
                                newFile.WriteLine(retractionCount % 2 == 0 ? "G10" : "G11");
                                retractionCount++;
                            }
                            else
                            {
                                newFile.WriteLine(line);
                            }
                        }

                    }


                }
            }
        }





    }
}
