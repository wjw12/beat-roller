package main

import (
	"encoding/csv"
	"fmt"
	"io/ioutil"
	"math/rand"
	"os"
	"strconv"
	"strings"
	"time"
)

func transform(filename string) (result bool) {
	file, err := ioutil.ReadFile(filename)
	if err != nil {
		fmt.Println("文件" + filename + "不存在")
		result = false
		return
	}
	flag := false
	count := 0
	var offset []int
	var perBeat []float64
	var type1 []byte
	var start []float64
	var end []float64
	var angle []int
	seed := time.Now().UTC().UnixNano()
	random := rand.New(rand.NewSource(seed))
	step := random.Intn(12) * 30
	step_increment := random.Intn(3)*30 + 30
	step_interval := random.Intn(10) + 10
	time := 0
	beat_position := 0
	spinner_flag := false
	spinner_start := 0.0
	spinner_end := 0.0
	repeat := 1.0
	jump_step := 0
	SliderMultiplier := 1.4
	step_temp := 0
	filepath := strings.Split(filename, ".osu")
	file_message, error := os.Create(filepath[0] + ".txt")
	if error != nil {
		fmt.Println(error)
	}
	for _, line := range strings.Split(string(file), "\n") {
		if count == 0 {
			for _, data := range strings.Split(string(line), " ") {
				if flag == true {
					file_message.WriteString("format :" + data + "\n")
					flag = false
					count++
					break
				}
				if strings.Contains(data, "format") {
					flag = true
				}
			}
			continue
		}
		if strings.Contains(line, "[Metadata]") {
			count++
			continue
		}
		if count == 2 {
			for _, data := range strings.Split(string(line), ":") {
				if strings.Contains(data, "[") {
					count++
					break
				}
				if data == "\r" || data == "\n" {
					continue
				}
				if strings.Contains(data, "Title") || strings.Contains(data, "Artist") || strings.Contains(data, "Creator") || strings.Contains(data, "Version") || strings.Contains(data, "Source") || strings.Contains(data, "Tags") {
					file_message.WriteString(line + "\n")
				}
			}
		}
		if count == 3 {
			for _, data := range strings.Split(string(line), ":") {
				if strings.Contains(data, "[TimingPoints]") {
					count++
					break
				}
				if flag == true {
					t1 := 0
					t2 := 0
					for k := 0; k < len(data); k++ {
						if data[k] >= '0' && data[k] <= '9' {
							t2 = k
							if data[t1] < '0' || data[t1] > '9' {
								t1 = k
							}
						}
					}
					if t1 == t2 {
						SliderMultiplier, _ = strconv.ParseFloat(string(data[t1]), 64)
					} else {
						SliderMultiplier, _ = strconv.ParseFloat(data[t1:t2+1], 64)
					}
					flag = false
				}
				if strings.Contains(data, "SliderMultiplier") {
					flag = true
				}
			}
			continue
		}
		if count == 4 {
			point := 0
			for _, data := range strings.Split(string(line), ",") {
				if strings.Contains(data, "[") {
					count++
					if strings.Contains(line, "[HitObjects]") {
						count++
					}
					break
				}
				if data == "\r" || data == "\n" {
					break
				}
				if flag == true {
					flag = false
					temp2 := 1.0
					if data[len(data)-1] == '\r' || data[len(data)-1] == '\n' {
						temp2, _ = strconv.ParseFloat(data[0:len(data)-1], 64)
					} else {
						temp2, _ = strconv.ParseFloat(data, 64)
					}
					if temp2 < 0 {
						temp2 = perBeat[point] * (-temp2) / 100
					} else {
						point = len(perBeat) - 1
					}
					perBeat = append(perBeat, temp2)
					break
				} else {
					temp1, _ := strconv.Atoi(data)
					offset = append(offset, temp1)
					flag = true
				}
			}
			continue
		}
		if strings.Contains(line, "[HitObjects]") {
			count++
			continue
		}
		if count == 6 {
			if time >= step_interval {
				time = 0
				if spinner_flag == false {
					step_interval = random.Intn(10) + 5
					step_increment = random.Intn(3)*30 + 30
				} else {
					step_increment = 30
				}
				if random.Intn(2) == 0 {
					step_increment = -step_increment
				}
				switch random.Intn(4) {
				case 0:
					jump_step = 60
				case 1:
					jump_step = 90
				case 2:
					jump_step = 120
				case 3:
					jump_step = 180
				}
				if random.Intn(2) == 0 && jump_step != 180 {
					jump_step = -jump_step
				}
				step = (step + jump_step + 360) % 360
			}
			position := 1
			type_temp := 0
			start_temp := 0.0
			for _, data := range strings.Split(string(line), ",") {
				if strings.Contains(data, "[") {
					count++
					break
				}
				if data == "\r" || data == "\n" {
					continue
				}
				if position == 3 {
					temp, _ := strconv.ParseFloat(data, 64)
					start_temp = temp
					if temp > spinner_end {
						spinner_flag = false
					}
				} else if position == 4 {
					temp, _ := strconv.Atoi(data)
					if temp%2 == 1 {
						if random.Intn(50) == 1 {
							spinner_start = start[len(start)-1]
							spinner_end = float64(random.Intn(3)+2)*1000.0 + spinner_start
							spinner_flag = true
							step_interval = 4
							time = 0
							step_increment = 30
							if random.Intn(2) == 0 {
								step_increment = -step_increment
							}
						}
						start = append(start, start_temp)
						step = (step + step_increment + 360) % 360
						angle = append(angle, step)
						time++
						end = append(end, 0)
						if spinner_flag == true {
							type1 = append(type1, 'S')
						} else {
							type1 = append(type1, 'N')
							if random.Intn(5) == 0 {
								start = append(start, start[len(start)-1])
								type1 = append(type1, 'N')
								switch random.Intn(3) {
								case 0:
									step_temp = 90
								case 1:
									step_temp = 120
								case 2:
									step_temp = 180
								}
								if random.Intn(2) == 0 && step_temp != 180 {
									step_temp = -step_temp
								}
								angle = append(angle, (step+step_temp+360)%360)
								end = append(end, 0)
							}
						}
						type_temp = 1
					} else if (temp/2)%2 == 1 {
						if random.Intn(50) == 1 {
							spinner_start = start[len(start)-1]
							spinner_end = float64(random.Intn(3)+2)*1000 + spinner_start
							spinner_flag = true
							step_interval = 4
							time = 0
							step_increment = 30
							if random.Intn(2) == 0 {
								step_increment = -step_increment
							}
						}
						start = append(start, start_temp)
						step = (step + step_increment + 360) % 360
						angle = append(angle, step)
						time++
						if spinner_flag == true {
							type1 = append(type1, 'S')
						} else {
							type1 = append(type1, 'L') //slider
						}
						for k := beat_position + 1; k < len(offset); k++ {

							if float64(offset[k]) > start[len(start)-1] {
								break
							}
							beat_position = k
						}
						type_temp = 2
					}
				} else if position == 7 && type_temp == 2 {
					if data[len(data)-1] == '\r' || data[len(data)-1] == '\n' {
						repeat, _ = strconv.ParseFloat(data[0:len(data)-1], 64)
					} else {
						repeat, _ = strconv.ParseFloat(data, 64)
					}

				} else if position == 8 && type_temp == 2 {
					pixelLength := 0.0
					if data[len(data)-1] == '\r' || data[len(data)-1] == '\n' {
						pixelLength, _ = strconv.ParseFloat(data[0:len(data)-1], 64)
					} else {
						pixelLength, _ = strconv.ParseFloat(data, 64)
					}
					end_time := start[len(start)-1] + pixelLength/(100.0*SliderMultiplier)*perBeat[beat_position]*repeat

					if spinner_flag == true {
						end = append(end, 0)
						start = append(start, end_time)
						if end_time > spinner_end {
							type1 = append(type1, 'N')
						} else {
							type1 = append(type1, 'S')
						}
						step = (step + step_increment + 360) % 360
						angle = append(angle, step)
						time++
						end = append(end, 0)
					} else {
						end = append(end, end_time)
						if random.Intn(5) == 0 {
							start = append(start, start[len(start)-1])
							type1 = append(type1, 'L')
							switch random.Intn(3) {
							case 0:
								step_temp = 90
							case 1:
								step_temp = 120
							case 2:
								step_temp = 180
							}
							if random.Intn(2) == 0 && step_temp != 180 {
								step_temp = -step_temp
							}
							angle = append(angle, (step+step_temp+360)%360)
							end = append(end, end_time)
						}
					}
				}
				position++
			}
		}
	}
	f, err := os.Create(filepath[0] + ".csv")
	if err != nil {
		panic(err)
	}
	defer f.Close()
	f.WriteString("\xEF\xBB\xBF")
	write := csv.NewWriter(f)
	writedata := make([]string, 4)
	for i := 0; i < len(type1); i++ {
		writedata[0] = string(type1[i])
		writedata[1] = fmt.Sprintf("%.5f", (start[i] / 1000.0))
		writedata[2] = strconv.Itoa(angle[i])
		writedata[3] = fmt.Sprintf("%.5f", (end[i] / 1000.0))
		write.Write(writedata)
	}
	write.Flush()
	file_message.Close()
	result = true
	return
}
