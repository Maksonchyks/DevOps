{{/*
Expand the name of the chart.
*/}}
{{- define "nutriplan.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
*/}}
{{- define "nutriplan.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "nutriplan.labels" -}}
helm.sh/chart: {{ include "nutriplan.name" . }}-{{ .Chart.Version }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
PostgreSQL connection string (uses bitnami chart service name)
*/}}
{{- define "nutriplan.postgresConnString" -}}
{{- printf "Host=%s-postgresql;Database=%s;Username=%s;Password=%s"
    .Release.Name
    .Values.postgresql.auth.database
    .Values.postgresql.auth.username
    .Values.postgresql.auth.password -}}
{{- end }}
